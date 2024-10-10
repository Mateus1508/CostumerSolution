using FluentValidation;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.API.Application.Validators
{
    public class CepValidator : AbstractValidator<string>
    {
        public CepValidator()
        {
            RuleFor(c => c)
                .NotEmpty().WithMessage("CEP não pode ser vazio.")
                .Length(8).WithMessage("CEP deve ter 8 dígitos.")
                .Matches("^[0-9]+$").WithMessage("CEP deve conter apenas dígitos.");
        }
    }
}
