using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.Database.Models;

namespace QuotingAPI.Database
{
    public class QuoteListDB : IQuoteListDB
    {
        private List<Quote> Quotes
        {
            get;
            set;
        }

        public QuoteListDB()
        {
            Quotes = new List<Quote>();
        }

        public List<QuoteProducts> GetAll()
        {
           /* List<QuoteProducts> quoteproduct = new List<QuoteProducts>()
            {
                new QuoteProducts() { QuoteCode = "COT-001", ProductCode = "SOCCER-001 ", Quantity = 24 },
                new QuoteProducts() { QuoteCode = "COT-001", ProductCode = "BASKET-001 ", Quantity = 24 }
            };


            return new List<Quote>()
            {
                new Quote() { QuoteCode = "COT-001", ClientCode = "MTR-64000001", Price = 200 , Sold = false , QuoteProducts = quoteproduct}
            };*/
            return new List<QuoteProducts>()
            {
               new QuoteProducts() { ProductCode = "SOCCER-001" , ClientCode = "MTR-64000001" , Quantity = 24  }, 
               new QuoteProducts() { ProductCode = "SOCCER-002" , ClientCode = "AMM-64000001" , Quantity = 13  }, 
               new QuoteProducts() { ProductCode = "BASKET-001" , ClientCode = "APR-24000001" , Quantity = 5 }
            };        
        }
        public Quote AddNew(Quote newQuote)
        {
            Quotes.Add(newQuote);
            return newQuote;
        }

    }
}
