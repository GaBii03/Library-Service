using LibraryService.Models.Entities;

namespace LibraryService.BL.Interfaces
{
    public interface IReaderService
    {
        void Add(Reader? reader);
        List<Reader> GetAll();
        Reader? GetById(int id);
        void Delete(int id);
    }
}
