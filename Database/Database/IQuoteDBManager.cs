using QuotingAPI.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Database.Models
{
    public  interface IQuoteDBManager :  IDBManager
    {
        QuoteProducts AddNew(QuoteProducts newQuote);
        QuoteProducts Update(QuoteProducts quoteToUpdate);
        bool Delete(int code);
        List<QuoteProducts> GetAll();
    }
}
