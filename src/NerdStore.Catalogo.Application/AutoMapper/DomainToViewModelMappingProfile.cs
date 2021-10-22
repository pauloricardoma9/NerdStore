using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dimensoes => dimensoes.Largura, produtoViewModel => produtoViewModel.MapFrom(produto => produto.Dimensoes.Largura))
                .ForMember(dimensoes => dimensoes.Altura, produtoViewModel => produtoViewModel.MapFrom(produto => produto.Dimensoes.Altura))
                .ForMember(dimensoes => dimensoes.Profundidade, produtoViewModel => produtoViewModel.MapFrom(produto => produto.Dimensoes.Profundidade));
            CreateMap<Categoria, CategoriaViewModel>();
        }
    }
}
