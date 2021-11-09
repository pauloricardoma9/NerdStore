using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Extensions;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : 
        IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>,
        IRequestHandler<IniciarPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository,
                                    IMediatorHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand command, CancellationToken cancellationToken)
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

        public async Task<bool> Handle(AtualizarItemPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) { return false; }

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(command.ClienteId);

            if(pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, command.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, command.Quantidade);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(pedido.ClienteId, pedido.Id, command.ProdutoId, command.Quantidade));

            _pedidoRepository.AtualizarItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) { return false; }

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(command.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, command.ProdutoId);

            if (pedido != null && !pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(pedido.ClienteId, pedido.Id, command.ProdutoId));

            _pedidoRepository.RemoverItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand command, CancellationToken cancellationToken)
        {
            if(!ValidarComando(command)) { return false; }

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(command.ClienteId);

            if(pedido == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var voucher = await _pedidoRepository.ObterVoucherPorCodigo(command.CodigoVoucher);

            if(voucher == null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado!"));
                return false;
            }

            var voucherAplicacaoValidation = pedido.AplicarVoucher(voucher);

            if (!voucherAplicacaoValidation.IsValid)
            {
                foreach(var error in voucherAplicacaoValidation.Errors)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(command.ClienteId, pedido.Id, voucher.Id));

            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) { return false; }

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(command.ClienteId);
            pedido.IniciarPedido();

            var itensList = new List<Item>();
            pedido.PedidoItens.ForEach(item => itensList.Add(new Item { Id = item.ProdutoId, Quantidade = item.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.Id, pedido.ClienteId, pedido.ValorTotal, listaProdutosPedido, command.NomeCartao, command.NumeroCartao, command.ExpiracaoCartao, command.CvvCartao));

            _pedidoRepository.Atualizar(pedido);
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
