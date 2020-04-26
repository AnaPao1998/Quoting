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
                _quotesLogic.UpdateQuoteId(quoteId, updatedQuote);
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
                _quotesLogic.UpdateQuoteName(quoteName, updatedQuote);
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
