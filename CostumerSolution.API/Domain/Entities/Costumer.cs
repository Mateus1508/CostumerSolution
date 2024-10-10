using CostumerSolution.API.Domain.Enums;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.API.Domain.Entities
{
    public class Costumer
    {
        public CNPJ Cnpj {  get; set; }
        public string Nome { get; set; }
        public CostumerStatus Status { get; set; }
        public List<Endereco> Enderecos { get; set; }
        public List<Telefone> Telefones { get; set; }
        public List<Email> Emails { get; set; }
    }
}
