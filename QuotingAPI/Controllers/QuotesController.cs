using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotingAPI.DTOModels;
using QuotingAPI.BusinessLogic;

namespace QuotingAPI.Controllers
{
    //    [Route("api/[controller]")]
    [ApiController]
    [Route("api/quoting")]

    public class QuotesController : ControllerBase
    {
        private readonly IQuotesLogic _quotesLogic;

        public QuotesController(IQuotesLogic quoteslogic)
        {
            _quotesLogic = quoteslogic;
        }
        // GET: api/Quotes
        [HttpGet]
        public IEnumerable<QuoteDTO> GetAll()
        {
            return _quotesLogic.GetQuoteList();
        }

        // POST: api/Quotes
        [HttpPost]
        public void Post([FromBody] QuoteDTO newQuoteDTO)
        {
            Console.WriteLine("QUOTE - " + newQuoteDTO.QuoteName + ", id: " + newQuoteDTO.QuoteID +
                ", Client code: " + newQuoteDTO.ClientCode + ", Estado venta: " + newQuoteDTO.IsSell);
            List<QuoteProductsDTO> qp = new List<QuoteProductsDTO>();
            qp = newQuoteDTO.QuoteLineItems;
            foreach (QuoteProductsDTO quop in qp)
            {
                Console.WriteLine("Codigo producto: " + quop.ProductCode + ", Cantidad: " + quop.Quantity + ", Precio: "
                    + quop.Price);
            }

            _quotesLogic.AddNewQuote(newQuoteDTO);
        }
        // PUT: api/Quotes
        [HttpPut("id/{quoteId}")] //update by id
        public void Put(int quoteId, [FromBody] QuoteDTO updatedQuote)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("put by id => Id: " + quoteId + ", Name: " + updatedQuote.QuoteName + ", SellState: " + updatedQuote.IsSell + ", ClientCode: " + updatedQuote.ClientCode);
                _quotesLogic.UpdateQuote(quoteId, updatedQuote);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }

        // PUT: api/Quotes
        [HttpPut("name/{quoteName}")] //update by name
        public void Put(string quoteName, [FromBody] QuoteDTO updatedQuote)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteName == quoteName);
            if (exists != null)
            {
                Console.WriteLine("put by name => Name: " + quoteName + ", SellState: " + updatedQuote.IsSell + ", ClientCode: " + updatedQuote.ClientCode);
                _quotesLogic.UpdateQuote(quoteName, updatedQuote);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }
        [HttpPut("id/{quoteId}/sell")] //do the sale by id
        public void PutSell(int quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("Sell => Name: " + quoteId);
                _quotesLogic.UpdateSale(quoteId,true);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }
        [HttpPut("name/{quoteName}/sell")] //do the sale by name
        public void PutSell(string quoteName)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteName == quoteName);
            if (exists != null)
            {
                Console.WriteLine("Sell => Name: " + quoteName);
                _quotesLogic.UpdateSale(quoteName,true);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }
        [HttpPut("id/{quoteId}/cancel-sell")] //do the sale by id
        public void PutCancelSell(int quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("Sell => Name: " + quoteId);
                _quotesLogic.UpdateSale(quoteId, false);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }
        [HttpPut("name/{quoteName}/cancel-sell")] //do the sale by name
        public void PutCancelSell(string quoteName)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteName == quoteName);
            if (exists != null)
            {
                Console.WriteLine("Sell => Name: " + quoteName);
                _quotesLogic.UpdateSale(quoteName, false);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }




        /*
                // GET: api/Quotes/5
                [HttpGet("{id}", Name = "Get")]
                public string Get(int id)
                {
                    return "value";
                }


                // DELETE: api/ApiWithActions/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
                */
    }
}
