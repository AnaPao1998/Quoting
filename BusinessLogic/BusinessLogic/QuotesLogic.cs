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
            List<Quote> allQuotes = _quoteListDB.GetAll();

            List<QuoteDTO> allQuotesDTO = new List<QuoteDTO>();

            foreach (Quote quote in allQuotes)
            {
                List<QuoteProductsDTO> qplDto = new List<QuoteProductsDTO>();
                List<QuoteProducts> qpl = new List<QuoteProducts>();
                qpl = quote.QuoteLineItems;

                foreach (QuoteProducts qp in qpl)
                {
                    qplDto.Add(
                        new QuoteProductsDTO()
                        {
                            ProductCode = qp.ProductCode,
                            Quantity = qp.Quantity,
                            Price = qp.Price
                        });
                }
                allQuotesDTO.Add(
                    new QuoteDTO()
                    {
                        QuoteID = quote.QuoteID,
                        QuoteName = quote.QuoteName,
                        ClientCode = quote.ClientCode,
                        QuoteLineItems = qplDto,
                        IsSell = quote.IsSell
                    }
                );

            }

            return allQuotesDTO;
        }


        private void UpdateQuoteFunction(QuoteDTO updatedQuote, QuoteDTO quoteToUpdate) //General UpdateQuote Method
        {
            quoteToUpdate.IsSell = updatedQuote.IsSell;
            quoteToUpdate.QuoteName = updatedQuote.QuoteName;
            quoteToUpdate.QuoteLineItems = updatedQuote.QuoteLineItems;
            quoteToUpdate.ClientCode = updatedQuote.ClientCode;  //Update QuoteDTO List
            Quote upQuote = new Quote()
            {
                QuoteID = quoteToUpdate.QuoteID,
                ClientCode = quoteToUpdate.ClientCode,
                IsSell = quoteToUpdate.IsSell,
                QuoteName = quoteToUpdate.QuoteName
            };

            //QuoteItems Mapping
            List<QuoteProducts> quoteProducts = new List<QuoteProducts>();
            foreach (QuoteProductsDTO qpdto in quoteToUpdate.QuoteLineItems)
            {
                qpdto.Price = DiscountApplier(qpdto, qpdto.Price).Price;
                quoteProducts.Add
                (
                    new QuoteProducts()
                    {
                        ProductCode = qpdto.ProductCode,
                        Quantity = qpdto.Quantity,
                        Price = qpdto.Price
                    }
                );
            }
            upQuote.QuoteLineItems = quoteProducts;
            _quoteListDB.Update(upQuote);
        }
        private void UpdateQuoteSaleState(QuoteDTO quoteToUpdate) //General UpdateQuote Method
        {
            Quote upQuote = new Quote()
            {
                QuoteID = quoteToUpdate.QuoteID,
                ClientCode = quoteToUpdate.ClientCode,
                IsSell = quoteToUpdate.IsSell,
                QuoteName = quoteToUpdate.QuoteName
            };

            //QuoteItems Mapping
            List<QuoteProducts> quoteProducts = new List<QuoteProducts>();
            foreach (QuoteProductsDTO qpdto in quoteToUpdate.QuoteLineItems)
            {
                quoteProducts.Add
                (
                    new QuoteProducts()
                    {
                        ProductCode = qpdto.ProductCode,
                        Quantity = qpdto.Quantity,
                        Price = qpdto.Price
                    }
                );
            }
            upQuote.QuoteLineItems = quoteProducts;
            _quoteListDB.Update(upQuote);
        }
        public void UpdateQuote(string id, QuoteDTO updatedQuote)//update by id
        {
            List<QuoteDTO> quoteList = GetQuoteList();
            foreach (QuoteDTO quoteToUpdate in quoteList)
            {
                if (quoteToUpdate.QuoteID == id) //Search for quoteToUpdate by id
                {
                    UpdateQuoteFunction(updatedQuote, quoteToUpdate);
                }
            }
        }
        public void DeleteByID(string quoteID)
        {

            List<QuoteDTO> quoteList = GetQuoteList();

            var obj = quoteList.FirstOrDefault(q => q.QuoteID == quoteID);//Search for First quoteToDelete by id
            if (obj != null)
            {
                Quote delQuote = new Quote()
                {
                    QuoteID = obj.QuoteID,
                    ClientCode = obj.ClientCode,
                    IsSell = obj.IsSell,
                    QuoteName = obj.QuoteName
                };
                _quoteListDB.Delete(delQuote);
                quoteList.Remove(obj);
            }
        }

               public void UpdateSale(string id, bool state) //change SaleState by id
        {
            List<QuoteDTO> quoteList = GetQuoteList();
            foreach (QuoteDTO quoteToUpdate in quoteList)
            {

                if (quoteToUpdate.QuoteID == id) //Search for quoteToUpdate by id
                {
                    quoteToUpdate.IsSell = state;
                    UpdateQuoteSaleState(quoteToUpdate);
                }
            }
        }
        
        private QuoteProductsDTO DiscountApplier(QuoteProductsDTO quote, float price)
        {
            float quantityDiscount = 0;
            float rankingDiscount = 1 * (float)0.01; //Hardcoded ranking

            if (quote.Quantity >= 12)
            {
                if (quote.Quantity >= 24)
                    quantityDiscount = (float)0.10;
                else
                    quantityDiscount = (float)0.05;
            }
            quote.Price = (float)(price * (1 - quantityDiscount - rankingDiscount));//Applying Discounts to Final Price

            return quote;
        }

        public Quote generateCode(Quote groupQuote)
        {

            IEnumerable<Quote> quoteList = _quoteListDB.GetAll();
            int quoteID = quoteList.Count() + 1;

            groupQuote.QuoteID = "QUOTE-" + quoteID;

            return groupQuote;
        }

        public QuoteDTO AddNewQuote(QuoteDTO newQuote)
        {
            //cntId += 1;

            // Mappers
            Quote quote = new Quote();
            quote.QuoteID = generateCode(quote).QuoteID;
            // quote.QuoteID = new Random().Next(0, 99999);  //Making ID unique
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
                        Price = DiscountApplier(qtpDTO, qtpDTO.Price).Price

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


        /*  private List<QuoteDTO> GetEmptyList()
         {
             List<QuoteDTO> emptyList = new List<QuoteDTO>()
             {
                 new QuoteDTO() {QuoteName="Cotizacion Fair Play", ClientCode ="MTR-6400001" ,QuoteLineItems = new List<QuoteProductsDTO>(), IsSell = new Random().Next(2) == 1 },
                 new QuoteDTO() {QuoteName="Cotizacion Impulse", ClientCode="RHA-6400000", QuoteLineItems = new List<QuoteProductsDTO>(), IsSell = new Random().Next(2) == 1}
             };
             return emptyList;
         }

         private void AddToGroupList(QuoteProductsDTO quote, List<QuoteDTO> listsToAssign, string groupName)
         {
             QuoteDTO listToAssign = listsToAssign.Find(group => group.QuoteName.Contains(groupName.ToString()));
             float productInitialPrice = new Random().Next(50, 1000); // Initial Price
             QuoteProductsDTO newQuote =  DiscountApplier(quote, productInitialPrice* quote.Quantity);
              listToAssign.QuoteLineItems.Add(new QuoteProductsDTO() { ProductCode = newQuote.ProductCode, Quantity = newQuote.Quantity, Price = newQuote.Price });        
         }*/
    }
}
