# RabbitMQ z MassTransit PoC

## Przegląd

Projekt demonstracyjny pokazujący implementację komunikacji asynchronicznej między mikrousługami z wykorzystaniem:
- RabbitMQ jako brokera wiadomości
- MassTransit jako abstrakcji do komunikacji
- .NET 9 z Minimal API
- MediatR do separacji warstw aplikacji

Aplikacja składa się z dwóch mikrousług:
1. **Producer** - publikuje wiadomości do RabbitMQ
2. **Consumer** - odbiera i przetwarza wiadomości z RabbitMQ

## Architektura

```
┌──────────────┐         ┌──────────────┐         ┌──────────────┐
│              │ Publish │              │ Consume │              │
│   Producer   ├────────►│   RabbitMQ   ├────────►│   Consumer   │
│              │         │              │         │              │
└──────────────┘         └──────────────┘         └──────────────┘
```

### Technologie

- **RabbitMQ**: Broker wiadomości implementujący protokół AMQP
- **MassTransit**: Abstrakcja dla infrastruktury komunikacyjnej
- **.NET 9**: Framework aplikacji
- **MediatR**: Implementacja wzorca Mediator
- **PostgreSQL**: Baza danych (opcjonalnie)
- **Jaeger**: Distributed tracing (opcjonalnie)

## Uruchomienie

### Wymagania

- .NET 9 SDK
- Docker i Docker Compose

### Kroki

1. Uruchom infrastrukturę:
   ```bash
   docker-compose up -d
   ```

2. Zbuduj i uruchom Producer:
   ```bash
   cd src/Producer/WebApi
   dotnet run
   ```

3. Zbuduj i uruchom Consumer:
   ```bash
   cd src/Consumer/WebApi
   dotnet run
   ```

4. Wyślij testową wiadomość:
   ```bash
   curl -X POST http://localhost:5196/api/messages \
        -H "Content-Type: application/json" \
        -d '{"content": "Testowa wiadomość"}'
   ```

5. Sprawdź logi Consumera, czy wiadomość została odebrana.

## Diagnostyka

- **RabbitMQ Management UI**: http://localhost:15672 (guest/guest)
- **Jaeger UI**: http://localhost:16686

## Struktura projektu

```
├── docker-compose.yml
├── src/
│   ├── Producer/
│   │   ├── WebApi/
│   │   │   ├── Features/
│   │   │   │   └── Messages/
│   │   │   │       ├── SendMessageCommand.cs
│   │   │   │       ├── SendMessageCommandHandler.cs
│   │   │   │       └── MessageEndpoints.cs
│   │   │   ├── Program.cs
│   │   │   └── appsettings.json
│   │   └── Infrastructure/
│   │       ├── Messaging/
│   │       │   └── MessageService.cs
│   │       └── ProducerServiceCollectionExtensions.cs
│   ├── Consumer/
│   │   ├── WebApi/
│   │   │   ├── Features/
│   │   │   │   └── Messages/
│   │   │   │       ├── MessageReceivedNotification.cs
│   │   │   │       └── MessageReceivedHandler.cs
│   │   │   ├── Program.cs
│   │   │   └── appsettings.json
│   │   └── Infrastructure/
│   │       ├── Messaging/
│   │       │   └── YourMessageConsumer.cs
│   │       └── ConsumerServiceCollectionExtensions.cs
│   └── Contracts/
│       └── IYourMessage.cs
└── README.md
```

## Jak działa?

1. **Kontrakt wiadomości**:
   - `IYourMessage` definiuje strukturę wiadomości przesyłanej między usługami

2. **Proces produkcji**:
   - Klient wysyła żądanie na `/api/messages` z zawartością wiadomości
   - `SendMessageCommandHandler` obsługuje żądanie i wywołuje `MessageService`
   - `MessageService` publikuje wiadomość do RabbitMQ

3. **Proces konsumpcji**:
   - `YourMessageConsumer` odbiera wiadomość z RabbitMQ
   - Consumer loguje otrzymaną wiadomość
   - Opcjonalnie publikuje `MessageReceivedNotification` przez MediatR

## Rodzaje wymiany w RabbitMQ

W projekcie demonstracyjnie wykorzystano podstawowy rodzaj wymiany (Direct Exchange), ale RabbitMQ obsługuje także:

1. **Direct Exchange**: Kieruje wiadomości do kolejek na podstawie klucza routingu
2. **Topic Exchange**: Kieruje wiadomości według wzorców w kluczu routingu
3. **Fanout Exchange**: Kopiuje wiadomości do wszystkich przypisanych kolejek
4. **Headers Exchange**: Używa atrybutów nagłówkowych zamiast klucza routingu

## Rodzaje kolejek w RabbitMQ

RabbitMQ oferuje kilka typów kolejek:

1. **Kolejki standardowe (Classic Queues)**: Podstawowy typ kolejki
2. **Kolejki trwałe (Durable Queues)**: Przetrwają restart brokera
3. **Kolejki ulotne (Non-durable Queues)**: Istnieją tylko podczas działania brokera
4. **Kolejki quorum (Quorum Queues)**: Zaprojektowane pod kątem wysokiej dostępności
5. **Kolejki z priorytetem (Priority Queues)**: Wiadomości o wyższym priorytecie są dostarczane wcześniej

## Dobre praktyki

1. **Trwałość wiadomości**: Włącz dla ważnych wiadomości
2. **Potwierdzenia publikacji**: Używaj `rabbitConfig.UsePublisherConfirmation()`
3. **Ponowne próby**: Skonfiguruj politykę ponawiania dla konsumentów
4. **Monitorowanie**: Używaj Jaeger do śledzenia przepływu wiadomości
5. **Obsługa błędów**: Rozważ użycie kolejek dla wiadomości nieudanych (dead-letter queues)

## Rozszerzenia

Projekt można rozszerzyć o:
- Obsługę wielu typów wiadomości
- Implementację wzorca Saga dla długotrwałych procesów
- Dodanie walidacji wiadomości
- Implementację zabezpieczeń (szyfrowanie, autentykacja)
- Dodanie panelu administracyjnego do zarządzania kolejkami/wiadomościami

## Licencja

MIT
