using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class IniciarPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }
        public IniciarPedidoCommand(Guid clienteId, Guid pedidoId, decimal total, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }

        public override bool EhValido()
        {
            ValidationResult = new IniciarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class IniciarPedidoValidation : AbstractValidator<IniciarPedidoCommand>
    {
        public IniciarPedidoValidation()
        {
            RuleFor(command => command.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(command => command.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido inválido");

            RuleFor(command => command.NomeCartao)
                .NotEmpty()
                .WithMessage("O nome no cartão não foi informado");

            RuleFor(command => command.NumeroCartao)
                .CreditCard()
                .WithMessage("Número de cartão de crédito inválido");

            RuleFor(command => command.ExpiracaoCartao)
                .NotEmpty()
                .WithMessage("Data de expiração não informada");

            RuleFor(command => command.CvvCartao)
                .Length(3, 4)
                .WithMessage("O CVV não foi preenchido corretamente");
        }
    }
}
