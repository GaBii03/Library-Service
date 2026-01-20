using LibraryService.Models.Entities;

namespace LibraryService.DL.Interfaces
{
    public interface IReaderRepository
    {
        void Add(Reader? reader);
        List<Reader> GetAll();
        Reader? GetById(int id);
        void Delete(int id);
    }
}
