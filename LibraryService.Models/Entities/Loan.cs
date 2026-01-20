namespace LibraryService.Models.Entities
{
    public class Loan
    {
        public int Id { get; set; }

        public Book Book { get; set; }
        public Reader Reader { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; }
    }
}
