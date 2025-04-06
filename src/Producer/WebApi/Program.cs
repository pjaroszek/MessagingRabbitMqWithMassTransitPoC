using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure;
using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common;
using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Common.Interfaces.Mediator;
using Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Producer.Infrastructure.Message;
using Jaroszek.CoderHouse.Shared.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddMediator();
builder.Services.AddMessageBus(builder.Configuration);
builder.Services.AddTransient<MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/messages", async (SendMessageCommand command, IMediator mediator) =>
    {
        var result = await mediator.SendAsync(command);
        return Results.Ok(result);
    })
    .WithName("SendMessage")
    .WithOpenApi()
    .Produces<YourMessage>(StatusCodes.Status200OK);

app.Run();

