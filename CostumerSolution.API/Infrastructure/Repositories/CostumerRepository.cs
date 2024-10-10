using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.ValueObjects;
using CostumerSolution.API.Domain.Interfaces;
using CostumerSolution.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CostumerSolution.API.Infrastructure.Repositories
{
    public class CostumerRepository : ICostumerRepository
    {
        private readonly AppDbContext _context;

        public CostumerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Costumer> GetByCnpj(CNPJ cnpj, CancellationToken cancellationToken)
        {
            var costumer = await _context.Costumers
                .FirstOrDefaultAsync(c => c.Cnpj == cnpj, cancellationToken);

            return costumer;
        }

        public async Task<IEnumerable<Costumer>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Costumers
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> Add(Costumer costumer, CancellationToken cancellationToken)
        {
            await _context.Costumers.AddAsync(costumer, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> Update(Costumer costumer, CancellationToken cancellationToken)
        {
            var existingCostumer = await _context.Costumers
                .FirstOrDefaultAsync(c => c.Cnpj == costumer.Cnpj, cancellationToken);

            if (existingCostumer == null)
            {
                throw new InvalidOperationException("Costumer não encontrado.");
            }

            _context.Entry(existingCostumer).CurrentValues.SetValues(costumer);

            existingCostumer.Enderecos.Clear();
            foreach (var endereco in costumer.Enderecos)
            {
                existingCostumer.Enderecos.Add(endereco);
            }

            existingCostumer.Telefones.Clear();
            foreach (var telefone in costumer.Telefones)
            {
                existingCostumer.Telefones.Add(telefone);
            }

            existingCostumer.Emails.Clear();
            foreach (var email in costumer.Emails)
            {
                existingCostumer.Emails.Add(email);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> Delete(Costumer costumer, CancellationToken cancellationToken)
        {
            
            _context.Costumers.Remove(costumer);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
