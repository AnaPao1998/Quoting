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
        /*
                // GET: api/Quotes/5
                [HttpGet("{id}", Name = "Get")]
                public string Get(int id)
                {
                    return "value";
                }

                // PUT: api/Quotes/5
                [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }

                // DELETE: api/ApiWithActions/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
                */
    }
}
