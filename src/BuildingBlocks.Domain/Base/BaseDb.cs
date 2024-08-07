﻿using BuildingBlocks.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace BuildingBlocks.Domain.Base
{
    public class BaseDb : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DbContext> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseDb(
            IMediator mediator,
            ILogger<DbContext> logger,
            IHttpContextAccessor httpContextAccessor,
            DbContextOptions options
            ) : base(options)
        {
            _mediator = mediator;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Assembly.GetEntryAssembly().GetName().Name;
                }
                else
                {
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }
                if (entityEntry.State == EntityState.Modified)
                {

                    if (Entry((AuditableEntity)entityEntry.Entity).Property(p => p.IsDeleted).IsModified && Entry((AuditableEntity)entityEntry.Entity).Property(p => p.IsDeleted).CurrentValue)
                    {
                        ((AuditableEntity)entityEntry.Entity).DeletedAt = DateTime.UtcNow;
                        ((AuditableEntity)entityEntry.Entity).DeletedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Assembly.GetEntryAssembly().GetName().Name;
                    }
                    else
                    {
                        ((AuditableEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                        ((AuditableEntity)entityEntry.Entity).UpdatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? Assembly.GetEntryAssembly().GetName().Name;
                    }
                }
            }

            ValidateEntities();
            var savedEntities = await base.SaveChangesAsync(cancellationToken);
            await DispatchDomainEvents();
            return savedEntities;
        }

        private async Task DispatchDomainEvents()
        {
            var entities = ChangeTracker
                .Entries<IAggregateRoot>()
                .Select(x => x.Entity)
                .Where(e => e.DomainEvents.Count > 0);

            var domainEvents = entities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            entities.ToList().ForEach(x => x.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }

        private void ValidateEntities()
        {
            var entities = (from entry in ChangeTracker.Entries()
                            where entry.State == EntityState.Modified || entry.State == EntityState.Added
                            select entry);

            var validationResults = new List<ValidationResult>();

            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    StringBuilder stgBuilder = new StringBuilder();
                    foreach (var eve in validationResults)
                    {
                        stgBuilder.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            entity.Entity.GetType().Name, entity.State); ;
                        foreach (var ve in eve.MemberNames)
                        {
                            stgBuilder.AppendFormat("- Property: \"{0}\", Error: \"{1}\"",
                                ve, eve.ErrorMessage);
                        }
                    }
                    throw new Exception(stgBuilder.ToString());
                }
            }
        }
    }
}
