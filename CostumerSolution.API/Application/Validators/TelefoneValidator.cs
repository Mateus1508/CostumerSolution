using CostumerSolution.API.Domain.ValueObjects;
using FluentValidation;

namespace CostumerSolution.API.Application.Validators
{
    public class TelefoneValidator : AbstractValidator<Telefone>
    {
        public TelefoneValidator()
        {
            RuleFor(t => t.Value)
            .NotEmpty().WithMessage("Telefone não pode ser vazio.")
            .Matches(@"^\(?\d{2}\)? ?\d{4,5}-?\d{4}$").WithMessage("Telefone inválido.");
        }
    }
}
