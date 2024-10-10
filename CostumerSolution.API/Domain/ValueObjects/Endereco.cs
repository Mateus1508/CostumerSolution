using System.Text.RegularExpressions;

namespace CostumerSolution.API.Domain.ValueObjects
{
    public class Endereco
    {
        private readonly string _cep;
        private readonly string _logradouro;
        private readonly string _bairro;
        private readonly string _estado;
        private readonly string _cidade;
        private readonly string? _complemento;

        public Endereco(string cep, string logradouro, string bairro, string estado, string cidade, string? complemento = null)
        {
            _cep = CleanCep(cep);
            _logradouro = logradouro;
            _bairro = bairro;
            _estado = estado;
            _cidade = cidade;
            _complemento = complemento;
        }

        public string Cep => _cep;
        public string Logradouro => _logradouro;
        public string Bairro => _bairro;
        public string Estado => _estado;
        public string Cidade => _cidade;
        public string? Complemento => _complemento;

        private string CleanCep(string cep)
        {
            return Regex.Replace(cep, @"[^\d]", "");
        }

        public override bool Equals(object? obj)
        {
            if (obj is Endereco other)
            {
                return _cep == other._cep &&
                       _logradouro == other._logradouro &&
                       _bairro == other._bairro &&
                       _estado == other._estado &&
                       _cidade == other._cidade &&
                       _complemento == other._complemento;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_cep, _logradouro, _bairro, _estado, _cidade, _complemento);
        }
    }
}
