using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NerdStore.Catalogo.Application.ViewModels
{
    public class ProdutoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CategoriaId { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Descricao { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool Ativo { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Valor { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataCadastro { get; private set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Imagem { get; private set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int QuantidadeEstoque { get; private set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Altura { get; private set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Largura { get; private set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Profundidade { get; set; }

        public IEnumerable<CategoriaViewModel> Categorias { get; set; }
    }
}
