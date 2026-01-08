namespace ServiceLayer.Borrowings;

public interface IBorrowService
{
    bool Borrow(int borrowerId, int lenderId, IEnumerable<int> ids);
}

public class BorrowService
{
}