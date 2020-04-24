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
            float productInitialPrice = new Random().Next(150, 600); // Val Aleatorio inicial
            float quantityDiscount = 0;
            float rankingDiscount = 1 * (float)0.01; //Hardcoded ranking

            if (quote.Quantity >= 12)
            {
                if (quote.Quantity >= 24)
                    quantityDiscount = (float)0.10;
                else
                    quantityDiscount = (float)0.05;
            }
            quote.Price = (float)(productInitialPrice * quote.Quantity * (1 - quantityDiscount - rankingDiscount)); //Applying Discounts to Final Price

            listToAssign.QuoteLineItems.Add(new QuoteProductsDTO() { ProductCode = quote.ProductCode, ClientCode = quote.ClientCode, Quantity = quote.Quantity, Price = quote.Price, IsSell = quote.IsSell });
        }

        public void AddNewQuote(QuoteDTO newQuote)
        {
            // Mappers
            Quote quote = new Quote();
            quote.QuoteName = newQuote.QuoteName;

            // Matching lists QuoteProductsDTO to QuoteProducts
            List<QuoteProducts> quoteList = new List<QuoteProducts>();
            List<QuoteProductsDTO> qp = new List<QuoteProductsDTO>();
            qp = newQuote.QuoteLineItems;
            foreach (QuoteProductsDTO qtpDTO in qp)
            {
                quoteList.Add
                (
                    new QuoteProducts()
                    {
                        ProductCode = qtpDTO.ProductCode,
                        ClientCode = qtpDTO.ClientCode,
                        Quantity = qtpDTO.Quantity,
                        Price = qtpDTO.Price,
                        IsSell = qtpDTO.IsSell
                    }
                );
            }
            // quote.QuoteLineItems Matched
            quote.QuoteLineItems = quoteList;
            
            // Add to DB
            _quoteListDB.AddNew(quote);
        }
    }
}
