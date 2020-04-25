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
        private int cntId = 0;

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

        public QuoteDTO AddNewQuote(QuoteDTO newQuote)
        {
            cntId += 1;
            // Mappers
            Quote quote = new Quote();
            quote.QuoteID = cntId; //Making ID unique
            quote.QuoteName = newQuote.QuoteName;
            quote.ClientCode = newQuote.ClientCode;
            quote.IsSell = newQuote.IsSell;

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
                        Quantity = qtpDTO.Quantity,
                        Price = qtpDTO.Price
                    }
                );
            }
            // quote.QuoteLineItems Matched
            quote.QuoteLineItems = quoteList;
            
            // Add to DB
            Quote quoteInDB = _quoteListDB.AddNew(quote);

            //Mapping back to DTO
            QuoteDTO quoteInDTO = new QuoteDTO();

            quoteInDTO.QuoteID = quoteInDB.QuoteID;
            quoteInDTO.QuoteName = quoteInDB.QuoteName;
            quoteInDTO.ClientCode = quoteInDB.ClientCode;
            quoteInDTO.IsSell = quoteInDB.IsSell;

            //Mapping back QuoteLineItems
            List<QuoteProducts> quoteListBack = new List<QuoteProducts>();
            List<QuoteProductsDTO> qpBack = new List<QuoteProductsDTO>();
            quoteListBack = quoteInDB.QuoteLineItems;
            foreach (QuoteProducts qtp in quoteListBack)
            {
                qpBack.Add
                (
                    new QuoteProductsDTO()
                    {
                        ProductCode = qtp.ProductCode,
                        Quantity = qtp.Quantity,
                        Price = qtp.Price
                    }
                );
            }
            // quote.QuoteLineItems Matched
            quoteInDTO.QuoteLineItems = qpBack;

            return quoteInDTO;
        }
    }
}
