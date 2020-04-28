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

        public List<Quote> GetAll()
        {
            return Quotes;
        }
        public Quote AddNew(Quote newQuote)
        {
            Quotes.Add(newQuote);
            return newQuote;
        }
        public void Update(Quote updatedQuote)
        {
            var obj = Quotes.FirstOrDefault(q => q.QuoteID == updatedQuote.QuoteID);
            if (obj != null)
            {
                obj.QuoteName = updatedQuote.QuoteName;
                obj.ClientCode = updatedQuote.ClientCode;
                obj.QuoteLineItems = updatedQuote.QuoteLineItems;
                obj.IsSell = updatedQuote.IsSell;
            }
        }

        public void Delete(Quote deletedQuote)
        {
            var obj = Quotes.FirstOrDefault(q => q.QuoteID == deletedQuote.QuoteID);
            if (obj != null)
            {
                Quotes.Remove(obj);
            }

        }
    }
}
