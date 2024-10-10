namespace CostumerSolution.API.Application.DTOs
{
    public class AddressDTO
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string? Complemento { get; set; }

        public AddressDTO(string cep, string logradouro, string bairro, string estado, string cidade, string? complemento = null)
        {
            Cep = cep;
            Logradouro = logradouro;
            Bairro = bairro;
            Estado = estado;
            Cidade = cidade;
            Complemento = complemento;
        }
    }
}
