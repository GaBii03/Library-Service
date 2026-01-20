using LibraryService.Models.Entities;

namespace LibraryService.DL.Interfaces
{
    public interface ILoanRepository
    {
        void AddLoan(Loan loan);
        Loan? GetLoanById(int id);
        List<Loan> GetAllLoans();
        void UpdateLoan(Loan loan);
        void DeleteLoan(int id);
        List<Loan> GetLoansByReaderId(int readerId);
        List<Loan> GetLoansByBookId(int bookId);
    }
}
