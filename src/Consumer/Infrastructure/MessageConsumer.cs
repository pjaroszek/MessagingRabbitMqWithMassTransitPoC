using Jaroszek.CoderHouse.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Jaroszek.CoderHouse.MessagingRabbitMqWithMassTransitPoC.Consumer.Infrastructure;

public class MessageConsumer : IConsumer<IYourMessage>
{
    private readonly ILogger<MessageConsumer> _logger;

    public MessageConsumer(ILogger<MessageConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IYourMessage> context)
    {
        var message = context.Message;
        int retryAttempt = context.GetRetryAttempt();
    
        _logger.LogInformation(
            "ROZPOCZĘTO PRZETWARZANIE: Id={MessageId}, Content={Content}, RetryAttempt={RetryAttempt}, Time={Timestamp}", 
            message.Id, 
            message.Content,
            retryAttempt,
            DateTime.Now.ToString("HH:mm:ss.fff"));
        
        if (message.Content.Contains("fail"))
        {
            _logger.LogWarning(
                "WYKRYTO BŁĄD: Id={MessageId}, RetryAttempt={RetryAttempt}, Time={Timestamp}", 
                message.Id,
                retryAttempt,
                DateTime.Now.ToString("HH:mm:ss.fff"));
            
            throw new ApplicationException($"Symulowany błąd dla wiadomości z 'fail'");
        }
    
        _logger.LogInformation(
            "ZAKOŃCZONO PRZETWARZANIE: Id={MessageId}, RetryAttempt={RetryAttempt}, Time={Timestamp}", 
            message.Id,
            retryAttempt,
            DateTime.Now.ToString("HH:mm:ss.fff"));
    }
    
    private async Task<bool> ProcessMessage(IYourMessage message)
    {
        // Tutaj umieść właściwą logikę biznesową przetwarzania wiadomości
        // Na przykład: zapisanie do bazy danych, wywołanie innej usługi, itp.
        
        // Symuluj długotrwałe przetwarzanie
        await Task.Delay(500);
        
        // Symulujemy różne wyniki przetwarzania:
        if (message.Content.Contains("error"))
        {
            throw new Exception("Wykryto słowo kluczowe 'error' w treści wiadomości.");
        }
        
        if (message.Content.Contains("fail"))
        {
            return false; // Niepowodzenie bez wyjątku
        }
        
        _logger.LogInformation("Wiadomość przetworzona pomyślnie: {MessageId}", message.Id);
        
        return true; // Powodzenie
    }
}