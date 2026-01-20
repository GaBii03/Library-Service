namespace LibraryService.Models.Configurations
{
    public class MongoDbConfiguration
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
    }
}
