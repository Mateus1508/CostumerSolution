using System.Text.RegularExpressions;

namespace CostumerSolution.API.Domain.ValueObjects
{
    public class CNPJ
    {
        public string Value { get; set; }

        public CNPJ()
        {
            Value = string.Empty;
        }

        public CNPJ(string value) : this()
        {
            value = Uri.UnescapeDataString(value);
            Value = CleanCnpj(value);
        }

        private string CleanCnpj(string value)
        {
            return Regex.Replace(value, @"\D", "");
        } 

        public override bool Equals(object obj)
        {
            if (obj is CNPJ otherCnpj)
            {
                return Value == otherCnpj.Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
