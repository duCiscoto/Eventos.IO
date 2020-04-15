using Eventos.IO.Domain.Organizadores;
using Eventos.IO.Infra.Data.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventos.IO.Infra.Data.Mappings
{
    public class OrganizadorMapping : EntityTypeConfiguration<Organizador>
    {
        public override void Map(EntityTypeBuilder<Organizador> builder)
        {
            builder.Property(o => o.Nome)
                .HasColumnType("varchar(150)")
                .IsRequired();

            builder.Property(o => o.Email)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(o => o.CPF)
                .HasColumnType("varchar(11)")
                .HasMaxLength(11)
                .IsRequired();

            builder.Ignore(c => c.ValidationResult);

            builder.Ignore(c => c.CascadeMode);

            builder.ToTable("Organizadores");
        }
    }
}
