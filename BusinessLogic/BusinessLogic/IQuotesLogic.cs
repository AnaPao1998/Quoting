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

        void UpdateQuote(int id,QuoteDTO updatedQuote);
        void UpdateQuote(string name,QuoteDTO updatedQuote);

        void UpdateSale(int id, bool state);
        void UpdateSale(string name, bool state);
    }
}
