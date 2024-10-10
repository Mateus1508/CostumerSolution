using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CostumerSolution.API.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Costumer> Costumers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCostumer(modelBuilder.Entity<Costumer>());
        }

        private void ConfigureCostumer(EntityTypeBuilder<Costumer> entity)
        {
            entity.HasKey(c => c.Cnpj);

            entity.Property(c => c.Cnpj)
                .HasConversion
                (
                    c => c.Value,
                    v => new CNPJ(v)
                )
                .HasMaxLength(14);

            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(c => c.Status)
                .IsRequired();

            ConfigureTelefones(entity);
            ConfigureEmails(entity);
            ConfigureEnderecos(entity);
        }

        private void ConfigureTelefones(EntityTypeBuilder<Costumer> entity)
        {
            entity.OwnsMany(c => c.Telefones, telefone =>
            {
                telefone.WithOwner().HasForeignKey("CostumerCnpj");

                telefone.Property(t => t.Value)
                         .IsRequired()
                         .HasMaxLength(11);

            });
        }

        private void ConfigureEmails(EntityTypeBuilder<Costumer> entity)
        {
            entity.OwnsMany(c => c.Emails, email =>
            {
                email.WithOwner()
                    .HasForeignKey("CostumerCnpj");

                email.Property(e => e.Value)
                     .IsRequired()
                     .HasMaxLength(100);
            });
        }

        private void ConfigureEnderecos(EntityTypeBuilder<Costumer> entity)
        {
            entity.OwnsMany(c => c.Enderecos, endereco =>
            {
                endereco.WithOwner().HasForeignKey("CostumerCnpj");

                endereco.Property(e => e.Cep)
                    .IsRequired()
                    .HasMaxLength(8);

                endereco.Property(e => e.Logradouro)
                    .IsRequired()
                    .HasMaxLength(200);

                endereco.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(200);

                endereco.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(2);

                endereco.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(200);

                endereco.Property(e => e.Complemento)
                    .HasMaxLength(200);
            });
        }
    }
}
