using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.API.Domain.Interfaces
{
    public interface ICostumerRepository
    {
        Task<Costumer> GetByCnpj(CNPJ cnpj, CancellationToken cancellationToken);
        Task<IEnumerable<Costumer>> GetAll(CancellationToken cancellationToken);
        Task<bool> Add(Costumer cliente, CancellationToken cancellationToken);
        Task<bool> Update(Costumer cliente, CancellationToken cancellationToken);
        Task<bool> Delete(Costumer cliente, CancellationToken cancellationToken);
    }
}
