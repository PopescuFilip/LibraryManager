using FluentValidation;

namespace ServiceLayer.BookEditions;

public class BookAddOptionsValidator : AbstractValidator<BookAddOptions>
{
    public BookAddOptionsValidator()
    {
        RuleFor(x => x.ForReadingRoomCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ForBorrowingCount).GreaterThanOrEqualTo(0);
    }
}