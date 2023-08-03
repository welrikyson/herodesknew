using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using herodesknew.Local.Application.Tickets.Queries.GetTickets;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GetTicketsQueryHandler>();
builder.Services.AddTransient<OpenSqlFileCommandHandler>();
builder.Services.AddTransient<CreateSqlFileCommandHandler>();
//builder.Services
//            .Scan(
//                selector => selector
//                    .FromAssemblies(
//                        Infrastructure.AssemblyReference.Assembly)
//                    .AddClasses(false)
//                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
//                    .AsMatchingInterface()
//                    .WithScopedLifetime());

builder.Services.AddCors(
            options => options
                .AddPolicy(
                    "AllowAllOrigins",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
