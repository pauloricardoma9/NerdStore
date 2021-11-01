using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository,
                                    IMediatorHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) { return false; }

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(command.ClienteId);
            var pedidoItem = new PedidoItem(command.ProdutoId, command.Nome, command.Quantidade, command.ValorUnitario);

            if(pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(command.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(command.ClienteId, command.ProdutoId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido.PedidoItens.FirstOrDefault(item => item.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, command.ValorUnitario, command.Quantidade, command.ProdutoId, command.Nome));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command command)
        {
            if (command.EhValido()) { return true; }

            foreach(var error in command.ValidationResult.Errors)
            {
                _mediatorHandler.PublicarNotificacao(new DomainNotification(command.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
