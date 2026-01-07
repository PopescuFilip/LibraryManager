using FluentValidation;

namespace ServiceLayer.BookEditions;

public class BooksUpdateOptionsValidator : AbstractValidator<BooksUpdateOptions>
{
    public BooksUpdateOptionsValidator()
    {
        RuleFor(x => x.ForReadingRoomCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ForBorrowingCount).GreaterThanOrEqualTo(0);
    }
}