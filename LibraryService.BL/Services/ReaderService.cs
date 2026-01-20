using LibraryService.BL.Interfaces;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Entities;

namespace LibraryService.BL.Services
{
    internal class ReaderService : IReaderService
    {
        private readonly IReaderRepository _readerRepository;

        public ReaderService(
            IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public void Add(Reader? reader)
        {
            if (reader == null) return;

            // Generate ID if not set (you might want to use a sequence generator in production)
            if (reader.Id == 0)
            {
                var allReaders = _readerRepository.GetAll();
                reader.Id = allReaders.Count > 0 ? allReaders.Max(r => r.Id) + 1 : 1;
            }

            _readerRepository.Add(reader);
        }
        

        public List<Reader> GetAll()
        {
            return _readerRepository.GetAll();
        }

        public Reader? GetById(int id)
        {
            return _readerRepository.GetById(id);
        }

        public void Delete(int id)
        {
            _readerRepository.Delete(id);
        }

        
    }
}
