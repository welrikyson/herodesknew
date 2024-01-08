using Cocona;
using herodesknew.Cli;

var builder = CoconaApp.CreateBuilder();
builder.Services.Install(builder.Configuration);
var app = builder.Build();

app.AddCommands<TicketCommands>();

app.Run();