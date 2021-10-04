using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Data.Mappings
{
    public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(categoria => categoria.Id);

            builder.Property(categoria => categoria.Nome)
                .IsRequired()
                .HasColumnType("varchar(250)");

            // 1:N => Categorias : Produtos
            builder.HasMany(categoria => categoria.Produtos)
                .WithOne(produto => produto.Categoria)
                .HasForeignKey(produto => produto.CategoriaId);

            builder.ToTable("Categorias");
        }
    }
}
