using FluentValidation;

namespace App.Application.Reminders.Create
{
    public class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand>
    {
        public CreateReminderCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.ReminderDateTime)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Reminder date-time must be in the future.");
        }
    }
}
