using FluentValidation;

namespace Application.Cards.Commands;

public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(255);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(4).MaximumLength(255);
        RuleFor(x => x.Attack).NotEmpty().InclusiveBetween(1, 255);
        RuleFor(x => x.Cost).NotEmpty().InclusiveBetween(1, 255);
        RuleFor(x => x.Health).NotEmpty().InclusiveBetween(1, 255);
    }
}