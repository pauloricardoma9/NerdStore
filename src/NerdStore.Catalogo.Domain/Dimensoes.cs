using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain
{
    public class Dimensoes
    {
        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal largura, decimal altura, decimal profundidade)
        {
            Validacoes.ValidarSeMaiorQue(largura, 0, "O campo Largura não pode ser menor ou igual a 0");
            Validacoes.ValidarSeMaiorQue(altura, 0, "O campo Altura não pode ser menor ou igual a 0");
            Validacoes.ValidarSeMaiorQue(profundidade, 0, "O campo Profundidade não pode ser menor ou igual a 0");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }
    }
}
