using AppointmentsManagerMs.BusinessLayer.Services.ManagementService;
using AppointmentsManagerMs.BusinessLayer.StateMachines;
using AppointmentsManagerMs.BusinessLayer.Utils.ServiceBus;
using AppointmentsManagerMs.DataAccessLayer;
using AppointmentsManagerMs.DataAccessLayer.Repository;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => 
{ 
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()); 
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter()); 
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<SagaAppointmentsManagerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SagasRepository"), m =>
    {
        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        m.MigrationsHistoryTable($"__{nameof(SagaAppointmentsManagerContext)}");
    }));

builder.Services.AddMassTransit(x =>
{
    x.AddSagaRepository<Appointment>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
            r.ExistingDbContext<SagaAppointmentsManagerContext>();
            r.LockStatementProvider = new PostgresLockStatementProvider();
        });
    x.AddSagaRepository<Reservation>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
            r.ExistingDbContext<SagaAppointmentsManagerContext>();
            r.LockStatementProvider = new PostgresLockStatementProvider();
        });

    x.SetKebabCaseEndpointNameFormatter();

    var entryAsssembly = Assembly.GetEntryAssembly();

    x.AddSagaStateMachines(entryAsssembly);
    x.AddSagas(entryAsssembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.SetCustomEntityNameFormatter();

        cfg.ConfigureJsonSerializerOptions(settings =>
        {
            settings.Converters.Add(new DateOnlyConverter());
            settings.Converters.Add(new TimeOnlyConverter());
            return settings;
        });

        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAppointmentManagerService, AppointmentManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await using var scope = app.Services.CreateAsyncScope();
    using var db = scope.ServiceProvider.GetService<SagaAppointmentsManagerContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
