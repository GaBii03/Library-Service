namespace LibraryService.Models.Requests
{
    public class AddLoanRequest
    {
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
