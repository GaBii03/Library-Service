using System;
using System.Collections.Generic;
using System.Linq;
using LibraryService.DL.Interfaces;
using LibraryService.Models.Configurations;
using LibraryService.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryService.DL.Repositories
{
    public class BookMongoRepository : IBookRepository
    {
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;
        private readonly ILogger<BookMongoRepository> _logger;

        private readonly IMongoCollection<Book> _booksCollection;

        public BookMongoRepository(IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
                                  ILogger<BookMongoRepository> logger,
                                  IMongoClient mongoClient)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;
            var database = mongoClient.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _booksCollection = database.GetCollection<Book>($"{nameof(Book)}s");
        }


        public void AddBook(Book book)
        {
            if (book == null) return;
            try
            {
                _booksCollection.InsertOne(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book");
                throw;
            }

        }

        public void DeleteBook(int id)
        {
            if (id <= 0) return;
            try
            {
                var result = _booksCollection.DeleteOne(b => b.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"No book found with id {id} to delete");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book");
                throw;
            }
        }

        public List<Book> GetAllBooks()
        {
            return _booksCollection.Find(_ => true).ToList();
        }

        public Book? GetBookById(int id)
        {
            if (id <= 0) return null;
            try
            {
                return _booksCollection.Find(b => b.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book by id");
                throw;
            }
        }

        public void UpdateBook(Book book)
        {
            _booksCollection.ReplaceOne(b => b.Id == book.Id, book);
        }
    }
}
