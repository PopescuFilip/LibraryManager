using FluentValidation;

namespace ServiceLayer.BookEditions;

public class BooksAddOptionsValidator : AbstractValidator<BooksAddOptions>
{
    public BooksAddOptionsValidator()
    {
        RuleFor(x => x.ForReadingRoomCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ForBorrowingCount).GreaterThanOrEqualTo(0);
    }
}