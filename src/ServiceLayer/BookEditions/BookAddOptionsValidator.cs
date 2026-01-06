using FluentValidation;

namespace ServiceLayer.BookEditions;

public class BookAddOptionsValidator : AbstractValidator<BookAddOptions>
{
    public BookAddOptionsValidator()
    {
        RuleFor(x => x.BooksForReadingRoomCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.BooksForBorrowingCount).GreaterThanOrEqualTo(0);
    }
}