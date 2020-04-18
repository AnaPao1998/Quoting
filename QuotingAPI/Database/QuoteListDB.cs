using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.Database.Models;

namespace QuotingAPI.Database
{
    public class QuoteListDB
    {
        public List<Quote> GetAll()

        {
            List<QuoteProducts> quoteproduct = new List<QuoteProducts>()
            {
                new QuoteProducts() { QuoteCode = "COT-001", ProductCode = "SOCCER-001 ", Quantity = 24 },
                new QuoteProducts() { QuoteCode = "COT-001", ProductCode = "BASKET-001 ", Quantity = 24 }
            };


            return new List<Quote>()
            {
                new Quote() { QuoteCode = "COT-001", ClientCode = "MTR-64000001", Price = 200 , Sold = false , QuoteProducts = quoteproduct}
            };
        }

    }
}
