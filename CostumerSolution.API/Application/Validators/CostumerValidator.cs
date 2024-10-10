using CostumerSolution.API.Domain.Entities;
using FluentValidation;

namespace CostumerSolution.API.Application.Validators
{
    public class CostumerValidator : AbstractValidator<Costumer>
    {
        public CostumerValidator()
        {
            RuleFor(c => c.Cnpj)
                .NotNull().WithMessage("CNPJ é obrigatório.")
                .SetValidator(new CnpjValidator());

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome não pode ser vazio.");

            RuleFor(c => c.Status)
            .IsInEnum().WithMessage("Status deve ser 0 (Inativo) ou 1 (Ativo).");

            RuleFor(c => c.Enderecos)
                .NotNull().WithMessage("Endereços não podem ser nulos.")
                .Must(enderecos => enderecos.Count > 0).WithMessage("Pelo menos um endereço deve ser fornecido.");

            RuleForEach(c => c.Enderecos)
                .SetValidator(new EnderecoValidator());

            RuleFor(c => c.Telefones)
                .NotNull().WithMessage("Telefones não podem ser nulos.")
                .Must(telefones => telefones.Count > 0).WithMessage("Pelo menos um telefone deve ser fornecido.");

            RuleForEach(c => c.Telefones)
                .SetValidator(new TelefoneValidator());

            RuleFor(c => c.Emails)
                .NotNull().WithMessage("Emails não podem ser nulos.")
                .Must(emails => emails.Count > 0).WithMessage("Pelo menos um email deve ser fornecido.");

            RuleForEach(c => c.Emails)
                .SetValidator(new EmailValidator());
        }
    }
}