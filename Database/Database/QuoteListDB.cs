using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using QuotingAPI.Database.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serilog;

namespace QuotingAPI.Database
{
    public class QuoteListDB : IQuoteListDB
    {
        private readonly IConfiguration _configuration;

        private string _dbPath;
        private DBContext _dbContext;

        private List<Quote> Quotes { get; set; }

        public QuoteListDB(IConfiguration configuration)
        {
            _configuration = configuration;
            InitDBContext();
        }

        public void InitDBContext()
        {
            _dbPath = _configuration.GetSection("Database").GetSection("connectionString").Value;
            Log.Logger.Information("test");
            _dbContext = JsonConvert.DeserializeObject<DBContext>(File.ReadAllText(_dbPath));
            Quotes = _dbContext.Quote;
        }

        public void SaveChanges()
        {
            File.WriteAllText(_dbPath, JsonConvert.SerializeObject(_dbContext));
        }
        public List<Quote> GetAll()
        {
            return Quotes;
        }
        public Quote AddNew(Quote newQuote)
        {
            Quotes.Add(newQuote);
            SaveChanges();
            return newQuote;
        }
        public Quote Update(Quote updatedQuote)
        {
            var obj = Quotes.FirstOrDefault(q => q.QuoteID == updatedQuote.QuoteID);
            if (obj != null)
            {
                obj.QuoteName = updatedQuote.QuoteName;
                obj.ClientCode = updatedQuote.ClientCode;
                obj.QuoteLineItems = updatedQuote.QuoteLineItems;
                obj.IsSell = updatedQuote.IsSell;
            }
            SaveChanges();
            return obj;


        }

        public bool Delete(Quote deletedQuote)
        {
            var obj = Quotes.FirstOrDefault(q => q.QuoteID == deletedQuote.QuoteID);
            if (obj != null)
            {
                Quotes.Remove(obj);
            }
            bool wasRemoved = Quotes.Remove(deletedQuote);
            SaveChanges();
            return wasRemoved;

        }
    }
}
