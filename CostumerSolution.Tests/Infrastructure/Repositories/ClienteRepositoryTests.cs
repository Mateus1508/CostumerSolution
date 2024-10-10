using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Domain.ValueObjects;
using CostumerSolution.API.Infrastructure.Persistence;
using CostumerSolution.API.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CostumerSolution.API.Tests.Infrastructure.Repositories
{
    public class CostumerRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ICostumerRepository _repository;

        public CostumerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CostumerTestDb")
                .Options;

            _context = new AppDbContext(options);
            _repository = new CostumerRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact(DisplayName = "Buscar cliente por CNPJ")]
        public async Task GetByCnpjShouldReturnCostumer()
        {
            var cnpj = new CNPJ("12345678000195");
            var costumer = new Costumer
            {
                Cnpj = cnpj,
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo,
                Enderecos = new List<Endereco>
                {
                    new Endereco("40726-000", "Rua teste", "Bairro teste", "BA", "Salvador")
                },
                Telefones = new List<Telefone>
                {
                    new Telefone("71 99999-9999")
                },
                Emails = new List<Email>
                {
                    new Email("cliente@gmail.com")
                }
            };

            await _context.Costumers.AddAsync(costumer);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByCnpj(cnpj, CancellationToken.None);

            result.Should().NotBeNull();
            result.Cnpj.Should().Be(cnpj);
            result.Nome.Should().Be("Cliente Teste");
            result.Status.Should().Be(CostumerStatus.Ativo);
            result.Enderecos.Should().HaveCount(1);
            result.Enderecos.First().Logradouro.Should().Be("Rua teste");
            result.Telefones.Should().HaveCount(1);
            result.Telefones.First().Value.Should().Be("71999999999");
            result.Emails.Should().HaveCount(1);
            result.Emails.First().Value.Should().Be("cliente@gmail.com");
        }

        [Fact(DisplayName = "Buscar todos os clientes")]
        public async Task GetAllShouldReturnAllCostumers()
        {
            var costumers = new List<Costumer>
            {
                new Costumer
                {
                    Cnpj = new CNPJ("12345678000195"),
                    Nome = "Cliente 1",
                    Status = CostumerStatus.Ativo,
                    Enderecos = new List<Endereco>
                    {
                        new Endereco("40726-000", "Rua 1", "Bairro 1", "BA", "Salvador")
                    },
                    Telefones = new List<Telefone>
                    {
                        new Telefone("71 99999-9999")
                    },
                    Emails = new List<Email>
                    {
                        new Email("cliente1@gmail.com")
                    }
                },
                new Costumer
                {
                    Cnpj = new CNPJ("98765432000196"),
                    Nome = "Cliente 2",
                    Status = CostumerStatus.Inativo,
                    Enderecos = new List<Endereco>
                    {
                        new Endereco("98765-432", "Rua 2", "Bairro 2", "RJ", "Rio de Janeiro")
                    },
                    Telefones = new List<Telefone>
                    {
                        new Telefone("21 91234-5678")
                    },
                    Emails = new List<Email>
                    {
                        new Email("cliente2@gmail.com")
                    }
                }
            };

            await _context.Costumers.AddRangeAsync(costumers);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAll(CancellationToken.None);

            result.Should().HaveCount(2);
            result.Should().Contain(costumer => costumer.Nome == "Cliente 1");
            result.Should().Contain(costumer => costumer.Nome == "Cliente 2");
        }

        [Fact(DisplayName = "Adicionar novo cliente deve retornar true em caso de sucesso")]
        public async Task AddShouldReturnTrueWhenCostumerIsAdded()
        {
            var costumer = new Costumer
            {
                Cnpj = new CNPJ("12345678000195"),
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo,
                Enderecos = new List<Endereco>
                {
                    new Endereco("40726-000", "Rua teste", "Bairro teste", "BA", "Salvador")
                },
                Telefones = new List<Telefone>
                {
                    new Telefone("71 99999-9999")
                },
                Emails = new List<Email>
                {
                    new Email("cliente@gmail.com")
                }
            };

            var result = await _repository.Add(costumer, CancellationToken.None);

            result.Should().BeTrue();
            _context.Costumers.Should().Contain(costumer);
        }

        [Fact(DisplayName = "Atualizar cliente deve retornar true em caso de sucesso")]
        public async Task UpdateShouldReturnTrueWhenCostumerIsUpdated()
        {
            var cnpj = new CNPJ("12345678000195");
            var costumer = new Costumer
            {
                Cnpj = cnpj,
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo,
                Enderecos = new List<Endereco>
                {
                    new Endereco("40726-000", "Rua teste", "Bairro teste", "BA", "Salvador")
                },
                Telefones = new List<Telefone>
                {
                    new Telefone("71 99999-9999")
                },
                Emails = new List<Email>
                {
                    new Email("cliente@gmail.com")
                }
            };

            await _context.Costumers.AddAsync(costumer);
            await _context.SaveChangesAsync();

            var updatedCostumer = new Costumer
            {
                Cnpj = cnpj,
                Nome = "Cliente Teste Atualizado",
                Status = CostumerStatus.Inativo,
                Enderecos = new List<Endereco>
                {
                    new Endereco("87654-321", "Rua Atualizada", "Bairro Atualizado", "BA", "Salvador")
                },
                Telefones = new List<Telefone>
                {
                    new Telefone("71 88888-8888")
                },
                Emails = new List<Email>
                {
                    new Email("cliente.atualizado@gmail.com")
                }
            };

            var result = await _repository.Update(updatedCostumer, CancellationToken.None);

            result.Should().BeTrue();
            var costumerAtualizado = await _repository.GetByCnpj(cnpj, CancellationToken.None);
            costumerAtualizado.Nome.Should().Be("Cliente Teste Atualizado");
            costumerAtualizado.Status.Should().Be(CostumerStatus.Inativo);
            costumerAtualizado.Enderecos.First().Logradouro.Should().Be("Rua Atualizada");
            costumerAtualizado.Telefones.First().Value.Should().Be("71888888888");
            costumerAtualizado.Emails.First().Value.Should().Be("cliente.atualizado@gmail.com");
        }

        [Fact(DisplayName = "Atualizar cliente deve retornar uma excessão em caso de cliente inexistente")]
        public async Task UpdateShouldThrowExceptionWhenCostumerDoesNotExist()
        {
            var costumerInexistente = new Costumer
            {
                Cnpj = new CNPJ("12345678000195"),
                Nome = "Cliente Inexistente"
            };

            Func<Task> act = async () => await _repository.Update(costumerInexistente, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Cliente não encontrado.");
        }

        [Fact(DisplayName = "Remover cliente deve retornar true em caso de sucesso")]
        public async Task DeleteShouldReturnTrueWhenCostumerIsDeleted()
        {
            var costumer = new Costumer
            {
                Cnpj = new CNPJ("12345678000195"),
                Nome = "Cliente Teste",
                Status = CostumerStatus.Ativo,
                Enderecos = new List<Endereco>
                {
                    new Endereco("40726-000", "Rua teste", "Bairro teste", "BA", "Salvador")
                },
                Telefones = new List<Telefone>
                {
                    new Telefone("71 99999-9999")
                },
                Emails = new List<Email>
                {
                    new Email("cliente@gmail.com")
                }
            };

            await _context.Costumers.AddAsync(costumer);
            await _context.SaveChangesAsync();

            var result = await _repository.Delete(costumer, CancellationToken.None);

            result.Should().BeTrue();
            _context.Costumers.Should().NotContain(costumer);
        }
    }
}
