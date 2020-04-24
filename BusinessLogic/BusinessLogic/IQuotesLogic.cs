using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.DTOModels;

namespace QuotingAPI.BusinessLogic
{
    public interface IQuotesLogic
    {
        public List<QuoteDTO> GetQuoteList();
        public void AddNewQuote(QuoteDTO newQuote);
    }
}
