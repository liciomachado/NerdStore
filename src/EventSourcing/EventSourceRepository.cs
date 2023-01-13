using EventStore.ClientAPI;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using System.Text;
using System.Text.Json;

namespace EventSourcing;

public class EventSourceRepository : IEventSourceRepository
{
    private readonly IEventStoreService _eventStoreService;

    public EventSourceRepository(IEventStoreService eventStoreService)
    {
        _eventStoreService = eventStoreService;
    }

    public async Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId)
    {
        var eventos = await _eventStoreService.GetConnection().ReadStreamEventsForwardAsync(
            aggregateId.ToString(), 0, 500, false
        );

        var listaEventos = new List<StoredEvent>();

        foreach (var resolvedEvent in eventos.Events)
        {
            var dataEncoded = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var jsonData = JsonSerializer.Deserialize<BaseEvent>(dataEncoded);

            var evento = new StoredEvent(
                resolvedEvent.Event.EventId,
                resolvedEvent.Event.EventType,
                jsonData.Timestamp,
                dataEncoded);

            listaEventos.Add(evento);
        }

        return listaEventos.OrderBy(e => e.DataOcorrencia);
    }

    public async Task SalvarEvent<TEvent>(TEvent evento) where TEvent : Event
    {
        await _eventStoreService.GetConnection().AppendToStreamAsync(
            evento.AggregateId.ToString(), ExpectedVersion.Any, FormatarEvento(evento));

    }

    private static IEnumerable<EventData> FormatarEvento<TEvent>(TEvent evento) where TEvent : Event
    {
        yield return new EventData(
            Guid.NewGuid(),
            evento.MessageType,
            true,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evento)),
            null
        );
    }
    internal class BaseEvent
    {
        public DateTime Timestamp { get; set; }
    }
}
