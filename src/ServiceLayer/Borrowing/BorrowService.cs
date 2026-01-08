namespace ServiceLayer.Borrowing;

public interface IBorrowService
{
    bool Borrow(int borrowerId, int lenderId, IdCollection bookIds);
}

public class BorrowService
{
}