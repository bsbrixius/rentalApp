using BuildingBlocks.Domain;
using BuildingBlocks.EventSourcing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BuildingBlocks.Infrastructure.Base
{
    public class BaseDb : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DbContext> _logger;

        public BaseDb(IMediator mediator, ILogger<DbContext> logger, DbContextOptions options) : base(options)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateEntities();
            var savedEntities = await base.SaveChangesAsync(cancellationToken);
            await PublishDomainEvents();
            return savedEntities;
        }

        //public override int SaveChanges()
        //{
        //    ValidateEntities();
        //    var savedEntities = base.SaveChanges();
        //    PublishDomainEvents().Wait();
        //    return savedEntities;
        //}

        private async Task PublishDomainEvents()
        {
            var entities = ChangeTracker
                .Entries<Entity>()
                .Select(x => x.Entity)
                .Where(e => e.DomainEvents.Count > 0);

            var domainEvents = entities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            entities.ToList().ForEach(x => x.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    //_logger.LogInformation("---- Sending domain event: {DomainEventName}: {@DomainEvent}",
                    //    domainEvent.GetGenericTypeName(),
                    //    domainEvent);

                    await _mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
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
