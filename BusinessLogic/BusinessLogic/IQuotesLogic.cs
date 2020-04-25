using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.DTOModels;

namespace QuotingAPI.BusinessLogic
{
    public interface IQuotesLogic
    {
        List<QuoteDTO> GetQuoteList();
        QuoteDTO AddNewQuote(QuoteDTO newQuote);
    }
}
