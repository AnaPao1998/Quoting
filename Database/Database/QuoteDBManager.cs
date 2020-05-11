using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using Database.Database.Models;
using QuotingAPI.Database.Models;

namespace Database.Database
{
    class QuoteDBManager : IQuoteDBManager
    {
        private readonly IConfiguration _configuration;

        private string _dbPath;
        private DBContext _dbContext;
        private List<QuoteProducts> _quoteList;

        public QuoteDBManager(IConfiguration configuration)
        {
            _configuration = configuration;
            InitDBContext();
        }

        public void InitDBContext()
        {
            _dbPath = _configuration.GetSection("Database").GetSection("connectionString").Value;
            _dbContext = JsonConvert.DeserializeObject<DBContext>(File.ReadAllText(_dbPath));
            _quoteList = _dbContext.QuoteProducts;
        }

        public void SaveChanges()
        {
            File.WriteAllText(_dbPath, JsonConvert.SerializeObject(_dbContext));
        }

        public List<QuoteProducts> GetAll()
        {
            return _quoteList;
        }

        public QuoteProducts AddNew(QuoteProducts newQuote)
        {
            // SaveChanges()
            throw new NotImplementedException();
        }
        public QuoteProducts Update(QuoteProducts quoteToUpdate)
        {
            // SaveChanges()
            throw new NotImplementedException();
        }
        public bool Delete(int code)
        {
            // SaveChanges()
            throw new NotImplementedException();
        }
    }
}
