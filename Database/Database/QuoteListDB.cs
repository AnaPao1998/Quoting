using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using QuotingAPI.Database.Models;
using System.Linq;
using Microsoft.Extensions.Logging;
using Serilog;
using Services;

namespace QuotingAPI.Database
{
    public class QuoteListDB : IQuoteListDB
    {
        private readonly IConfiguration _configuration;
        private readonly IPricingBookBackingService _productBackingService;

        private string _dbPath;
        private DBContext _dbContext;

        private List<Quote> Quotes { get; set; }

        public QuoteListDB(IConfiguration configuration, IPricingBookBackingService productBackingService)
        {
            _configuration = configuration;
            _productBackingService = productBackingService;
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
            UpdateQuotesBs();
        }
        public List<Quote> GetAll()
        {
            UpdateQuotesBs();
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

        public void UpdateQuotesBs()
        {
            List<ProductBsDTO> productos = _productBackingService.GetAllProduct().Result;
            foreach(ProductBsDTO p in productos)
            {
                foreach(Quote q in Quotes)
                {
                    foreach(QuoteProducts qp in q.QuoteLineItems)
                    {
                        if (qp.ProductCode == p.ProductCode)
                            qp.Price = (float)p.FixedPrice;
                    }
                }
            }
        }
    }
}
