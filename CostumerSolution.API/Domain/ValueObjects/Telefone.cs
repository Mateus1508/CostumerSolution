using System.Text.RegularExpressions;

namespace CostumerSolution.API.Domain.ValueObjects
{
    public class Telefone
    {
        public string Value { get; set; }

        public Telefone()
        {
            Value = string.Empty;
        }

        public Telefone(string numero) : this()
        {
            Value = CleanTelefone(numero);
        }

        private string CleanTelefone(string numero)
        {
            return Regex.Replace(numero, @"[^\d]", "");
        }

        public override bool Equals(object? obj)
        {
            if (obj is Telefone other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
