using NerdStore.Core.Messages;

namespace NerdStore.Core.Data.EventSourcing;

public interface IEventSourceRepository
{
    Task SalvarEvent<TEvent>(TEvent evento) where TEvent : Event;
    Task<IEnumerable<StoredEvent>> ObterEventos(Guid aggregateId);
}
