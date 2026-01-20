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
    public class LoanRepository : ILoanRepository
    {
        private readonly IOptionsMonitor<MongoDbConfiguration> _mongoDbConfiguration;
        private readonly ILogger<LoanRepository> _logger;
        private readonly IMongoCollection<Loan> _loansCollection;

        public LoanRepository(IOptionsMonitor<MongoDbConfiguration> mongoDbConfiguration,
                              ILogger<LoanRepository> logger,
                              IMongoClient mongoClient)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _logger = logger;
            var database = mongoClient.GetDatabase(_mongoDbConfiguration.CurrentValue.DatabaseName);
            _loansCollection = database.GetCollection<Loan>($"{nameof(Loan)}s");
        }

        public void AddLoan(Loan loan)
        {
            if (loan == null) return;
            try
            {
                _loansCollection.InsertOne(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding loan");
                throw;
            }
        }

        public void DeleteLoan(int id)
        {
            if (id <= 0) return;
            try
            {
                var result = _loansCollection.DeleteOne(l => l.Id == id);
                if (result.DeletedCount == 0)
                {
                    _logger.LogWarning($"No loan found with id {id} to delete");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting loan");
                throw;
            }
        }

        public List<Loan> GetAllLoans()
        {
            try
            {
                return _loansCollection.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all loans");
                throw;
            }
        }

        public Loan? GetLoanById(int id)
        {
            if (id <= 0) return null;
            try
            {
                return _loansCollection.Find(l => l.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving loan by id");
                throw;
            }
        }

        public void UpdateLoan(Loan loan)
        {
            try
            {
                _loansCollection.ReplaceOne(l => l.Id == loan.Id, loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating loan");
                throw;
            }
        }

        public List<Loan> GetLoansByReaderId(int readerId)
        {
            if (readerId <= 0) return new List<Loan>();
            try
            {
                return _loansCollection.Find(l => l.Reader.Id == readerId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loans by reader id");
                throw;
            }
        }

        public List<Loan> GetLoansByBookId(int bookId)
        {
            if (bookId <= 0) return new List<Loan>();
            try
            {
                return _loansCollection.Find(l => l.Book.Id == bookId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loans by book id");
                throw;
            }
        }
    }
}
