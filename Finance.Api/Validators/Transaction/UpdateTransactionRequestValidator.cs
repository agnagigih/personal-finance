using Finance.Api.DTOs.Transaction;
using FluentValidation;

namespace Personal.Finance.Api.Validators.Transaction
{
    public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
    {
        public UpdateTransactionRequestValidator() 
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.TransactionDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Transaction date cannot be in the future");

            RuleFor(x => x.Note)
                .MaximumLength(255)
                .When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
}
