using FluentValidation;
using CostumerSolution.API.Domain.Entities;
using CostumerSolution.API.Domain.ValueObjects;

namespace CostumerSolution.API.Application.Validators
{
    public class EnderecoValidator : AbstractValidator<Endereco>
    {
        public EnderecoValidator()
        {
            RuleFor(e => e.Cep)
                .SetValidator(new CepValidator());

            RuleFor(e => e.Estado)
                .NotEmpty().WithMessage("O estado não pode ser vazio.")
                .Length(2).WithMessage("O estado deve ter exatamente 2 caracteres.");

            RuleFor(e => e.Cidade)
                .NotEmpty().WithMessage("A cidade não pode ser vazia.");

            RuleFor(e => e.Bairro)
                .NotEmpty().WithMessage("O bairro não pode ser vazio.");

            RuleFor(e => e.Logradouro)
                .NotEmpty().WithMessage("O logradouro não pode ser vazio.");

        }
    }
}
