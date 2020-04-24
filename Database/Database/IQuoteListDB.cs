using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.Database.Models;

namespace QuotingAPI.Database
{
    public interface IQuoteListDB
    {
        public List<QuoteProducts> GetAll();
        public void AddNew(Quote newQuote);
    }
}
