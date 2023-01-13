using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.API.Controllers
{

    public abstract class ControllerMyBase : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediaTrHandler _mediaTrHandler;

        protected Guid ClienteId = Guid.Parse("a8a8db9a-6ca3-4d24-a10b-98f524322797");

        protected ControllerMyBase(MediatR.INotificationHandler<DomainNotification> notifications, IMediaTrHandler mediaTrHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediaTrHandler = mediaTrHandler;
        }

        protected bool OperacaoValida()
        {
            return (!_notifications.TemNotificacao());
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(x => x.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediaTrHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }
    }
}
