using Authentication.Domain.Domain;
using Authentication.Infraestructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//var AppName = typeof(Program).Assembly.FullName;
//builder.Host.AddSerilogCore();
//Log.Information("Starting web host");
//Log.Information(builder.Environment.EnvironmentName);
//Log.Information("Configuring web host ({ApplicationContext})...", AppName);
////Autofac Modules
//// Add services to the container.
//var container = new ContainerBuilder();
//container.Populate(builder.Services);
//container.RegisterModule(new MediatorModule());
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AuthenticationContext>()
    .AddApiEndpoints();

// Add services to the container.
builder.Services.AddDbContext<AuthenticationContext>();

//builder.Services.AddDbContext<AuthenticationContext>(options => options.UseInMemoryDatabase(nameof(AuthenticationContext)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Log.Information("Applying migrations ({ApplicationContext})...", AppName);
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        //dbContext.TryInitializeDatabaseTables(app.Configuration);
        //dbContext.TrySeedDatabase(app.Configuration);
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<User>();

app.Run();
