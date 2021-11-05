using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }

        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool EhValido()
        {
            ValidationResult = new AtualizarItemPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AtualizarItemPedidoValidation : AbstractValidator<AtualizarItemPedidoCommand>
        {
            public AtualizarItemPedidoValidation()
            {
                RuleFor(command => command.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(command => command.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(command => command.Quantidade)
                    .GreaterThan(0)
                    .WithMessage("A quantidade mínima de um item é 1");

                RuleFor(command => command.Quantidade)
                    .LessThan(16)
                    .WithMessage("A quantidade máxima de um item é 15");
            }
        }
    }
}
