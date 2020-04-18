using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotingAPI.DTOModels;

namespace QuotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Quotes
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
    }
}
