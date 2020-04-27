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
        void UpdateQuote(string id,QuoteDTO updatedQuote);
        void UpdateQuoteByName(string name,QuoteDTO updatedQuote);

        void UpdateSale(string id, bool state);
        void UpdateSaleByName(string name, bool state);

        void DeleteByID(string id);
    }
}
