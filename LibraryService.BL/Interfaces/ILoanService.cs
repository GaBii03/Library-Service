using LibraryService.Models.Entities;

namespace LibraryService.BL.Interfaces
{
    public interface ILoanService
    {
        void AddLoan(Loan loan);
        Loan? GetLoanById(int id);
        List<Loan> GetAllLoans();
        void UpdateLoan(Loan loan);
        void DeleteLoan(int id);
        List<Loan> GetLoansByReaderId(int readerId);
        List<Loan> GetLoansByBookId(int bookId);
        Loan? BorrowBook(int bookId, int readerId, DateTime dueDate);
        void ReturnBook(int loanId);
    }
}
