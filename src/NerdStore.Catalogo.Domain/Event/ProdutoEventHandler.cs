using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain.Event
{
    public class ProdutoEventHandler : 
        INotificationHandler<ProdutoAbaixoEstoqueEvent>,
        INotificationHandler<PedidoIniciadoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProdutoEventHandler(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMediatorHandler mediatorHandler)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent evento, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(evento.AggregateId);

            // Enviar um email para aquisição de mais produtos.
        }

        public async Task Handle(PedidoIniciadoEvent evento, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(evento.ProdutosPedido);

            if (result)
            {
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueConfirmadoEvent(evento.PedidoId, evento.ClienteId, evento.Total, evento.ProdutosPedido, evento.NomeCartao, evento.NumeroCartao, evento.ExpiracaoCartao, evento.CvvCartao));
            }
            else
            {
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueRejeitadoEvent(evento.PedidoId, evento.ClienteId));
            }
        }
    }
}
