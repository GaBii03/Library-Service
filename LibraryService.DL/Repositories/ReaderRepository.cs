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
    public class ReaderRepository : IReaderRepository
    {

        private readonly IMongoCollection<Reader> _readersCollection;
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;
        private readonly ILogger<ReaderRepository> _logger;

        public ReaderRepository(
            IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
            IMongoClient mongoClient,
            ILogger<ReaderRepository> logger)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;
            var database = mongoClient.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _readersCollection = database.GetCollection<Reader>($"{nameof(Reader)}s");
        }

        public void Add(Reader? reader)
        {
            if (reader == null) return;

            try
            {
                _readersCollection.InsertOneAsync(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reader");
                throw;
            }
        }

        public List<Reader> GetAll()
        {
            try
            {
                return _readersCollection.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all readers");
                throw;
            }
        }

        public Reader? GetById(int id)
        {
            if (id <= 0) return null;

            return _readersCollection.Find(r => r.Id == id).FirstOrDefault();
        }

        public void Delete(int id)
        {
            if (id <= 0) return;

            _readersCollection.DeleteOne(r => r.Id == id);
        }
    }
}
