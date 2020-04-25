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

    }
}
