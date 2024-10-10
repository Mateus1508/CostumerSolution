using CostumerSolution.API.Domain.ValueObjects;
using FluentValidation;

namespace CostumerSolution.API.Application.Validators
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(e => e.Value)
                .NotEmpty().WithMessage("O e-mail não pode ser vazio.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");
        }
    }
}
