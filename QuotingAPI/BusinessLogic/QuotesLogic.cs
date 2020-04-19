using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.DTOModels;
using QuotingAPI.Database;
using QuotingAPI.Database.Models;


namespace QuotingAPI.BusinessLogic
{
    public class QuotesLogic : IQuotesLogic
    {
        private readonly IQuoteListDB _quoteListDB;


        public QuotesLogic(IQuoteListDB quoteListDB)
        {
            _quoteListDB = quoteListDB;
        }
        public List<QuoteDTO> GetQuoteList()

        {

            List<QuoteProducts> allQuotes = _quoteListDB.GetAll();

            List<QuoteDTO> listToAssign = GetEmptyList();

            foreach (QuoteProducts quote in allQuotes)
            {
                string[] content = { "Fair Play", "Impulse" };
                string groupQuote = content[new Random().Next(content.Length)];
                AddToGroupList(quote, listToAssign, groupQuote);
            }

            return listToAssign;
        }

        private List<QuoteDTO> GetEmptyList()
        {
            List<QuoteDTO> emptyList = new List<QuoteDTO>()

            {
                new QuoteDTO() {QuoteName="Cotizacion Fair Play", QuoteLineItems = new List<QuoteProductsDTO>() },
                new QuoteDTO() {QuoteName="Cotizacion Impulse", QuoteLineItems = new List<QuoteProductsDTO>() }

            };
            return emptyList;
        }

        private void AddToGroupList(QuoteProducts quote, List<QuoteDTO> listsToAssign, string groupName)
        {
            QuoteDTO listToAssign = listsToAssign.Find(group => group.QuoteName.Contains(groupName.ToString()));
            quote.IsSell = new Random().Next(2) == 1;
            float productInitialPrice = 10; 

            if (quote.Quantity < 12)
                quote.Price = productInitialPrice * quote.Quantity;
            else
            {
                if (quote.Quantity < 24)
                    quote.Price = (float)(productInitialPrice * quote.Quantity * 0.05);
                else
                    quote.Price = (float)(productInitialPrice * quote.Quantity * 0.10);
            }

            listToAssign.QuoteLineItems.Add(new QuoteProductsDTO() { ProductCode = quote.ProductCode, ClientCode = quote.ClientCode, Quantity = quote.Quantity, Price = quote.Price, IsSell = quote.IsSell });
        }
    }
}
