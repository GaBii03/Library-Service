namespace LibraryService.Models.Requests
{
    public class AddBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
    }
}
