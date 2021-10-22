using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(produto =>
                    new Produto(produto.Nome, produto.Descricao, produto.Ativo, produto.Valor, 
                                produto.CategoriaId, produto.DataCadastro, produto.Imagem,
                                new Dimensoes(produto.Altura, produto.Largura, produto.Profundidade)));

            CreateMap<CategoriaViewModel, Categoria>()
                .ConstructUsing(categoria => new Categoria(categoria.Nome, categoria.Codigo));
        }
    }
}
