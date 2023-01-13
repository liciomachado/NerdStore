using MediatR;
using NerdStore.Core.Data.EventSourcing;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Core.Communication.Mediator
{
    public class MediaTrHandler : IMediaTrHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourceRepository _eventSourcingRepository;

        public MediaTrHandler(IMediator mediator, IEventSourceRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
            if (!evento.GetType().BaseType.Name.Equals("DomainEvent"))
                await _eventSourcingRepository.SalvarEvent(evento);
        }

        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification
        {
            await _mediator.Publish(notificacao);
        }
    }
}
