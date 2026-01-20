using LibraryService.BL.Interfaces;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;

namespace LibraryService.BL.Services
{
    internal class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReaderRepository _readerRepository;

        public LoanService(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            IReaderRepository readerRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
        }

        public void AddLoan(Loan loan)
        {
            if (loan == null) return;

            // Generate ID if not set
            if (loan.Id == 0)
            {
                var allLoans = _loanRepository.GetAllLoans();
                loan.Id = allLoans.Count > 0 ? allLoans.Max(l => l.Id) + 1 : 1;
            }

            _loanRepository.AddLoan(loan);
        }

        public Loan? GetLoanById(int id)
        {
            return _loanRepository.GetLoanById(id);
        }

        public List<Loan> GetAllLoans()
        {
            return _loanRepository.GetAllLoans();
        }

        public void UpdateLoan(Loan loan)
        {
            _loanRepository.UpdateLoan(loan);
        }

        public void DeleteLoan(int id)
        {
            _loanRepository.DeleteLoan(id);
        }

        public List<Loan> GetLoansByReaderId(int readerId)
        {
            return _loanRepository.GetLoansByReaderId(readerId);
        }

        public List<Loan> GetLoansByBookId(int bookId)
        {
            return _loanRepository.GetLoansByBookId(bookId);
        }

        public Loan? BorrowBook(int bookId, int readerId, DateTime dueDate)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null || !book.IsAvailable)
            {
                return null;
            }

            var reader = _readerRepository.GetById(readerId);
            if (reader == null)
            {
                return null;
            }

            var loan = new Loan
            {
                Book = book,
                Reader = reader,
                BorrowDate = DateTime.Now,
                DueDate = dueDate,
                IsReturned = false
            };

            // Generate ID
            var allLoans = _loanRepository.GetAllLoans();
            loan.Id = allLoans.Count > 0 ? allLoans.Max(l => l.Id) + 1 : 1;

            // Mark book as unavailable
            book.IsAvailable = false;
            _bookRepository.UpdateBook(book);

            _loanRepository.AddLoan(loan);
            return loan;
        }

        public void ReturnBook(int loanId)
        {
            var loan = _loanRepository.GetLoanById(loanId);
            if (loan == null || loan.IsReturned)
            {
                return;
            }

            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;

            // Mark book as available
            if (loan.Book != null)
            {
                loan.Book.IsAvailable = true;
                _bookRepository.UpdateBook(loan.Book);
            }

            _loanRepository.UpdateLoan(loan);
        }
    }
}
