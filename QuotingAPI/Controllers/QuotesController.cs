﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using QuotingAPI.DTOModels;
using QuotingAPI.BusinessLogic;
using QuotingAPI.Exceptions;

namespace QuotingAPI.Controllers
{
    [ApiController]
    [Route("api")]

    public class QuotesController : ControllerBase
    {
        private readonly IQuotesLogic _quotesLogic;
        private readonly IConfiguration _configuration;

        public QuotesController(IQuotesLogic quoteslogic, IConfiguration configuration)
        {
            _quotesLogic = quoteslogic;
            _configuration = configuration;
        }
        // GET: api/Quotes
        [HttpGet]
        [Route("quotes")]

        public IEnumerable<QuoteDTO> GetAll()
        {
            return _quotesLogic.GetQuoteList();
        }

        // POST: api/Quotes
        [HttpPost]
        [Route("quotes")]

        public QuoteDTO Post([FromBody] QuoteDTO newQuoteDTO)
        {
            var dbServer = _configuration.GetSection("Database").GetSection("connectionString");

            List<QuoteProductsDTO> qp = new List<QuoteProductsDTO>();
            qp = newQuoteDTO.QuoteLineItems;

            QuoteDTO newQuote = _quotesLogic.AddNewQuote(newQuoteDTO);

            Console.WriteLine("POST = > \t| QUOTE ID : " + $"{newQuote.QuoteID}" + " | NAME : " + newQuoteDTO.QuoteName +
                " | CLIENT CODE : " + newQuoteDTO.ClientCode + " | SALE STATUS : " + newQuoteDTO.IsSell);

            foreach (QuoteProductsDTO quop in qp)
            {
                Console.WriteLine("\t\t\t ->  | PRODUCT CODE : " + quop.ProductCode + " | QUANTITY : " + quop.Quantity +
                 " | PRICE : " + $"{quop.Price}" + "\n");
            }
            newQuote.QuoteID = $"{newQuote.QuoteID} data from { dbServer.Value }";

            return (newQuote);
        }

        // PUT: api/Quotes
        [HttpPut("quotes/{quoteId}")] //update by id
        public bool Put(string quoteId, [FromBody] QuoteDTO updatedQuote)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                _quotesLogic.UpdateQuote(quoteId, updatedQuote);
                return true;
            }
            else
            {
                throw new ControllerExceptions("The ID couldn't be found ! { " + quoteId + " }");
            }
            
        }

        [HttpPut("quotes/{quoteId}/sell")] //do the sale by id
        public bool PutSell(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                _quotesLogic.UpdateSale(quoteId, true);
                return true;
            }
            else
            {
                throw new ControllerExceptions("The ID couldn't be found ! { " + quoteId + " }");
            }
        }

        [HttpPut("quotes/{quoteId}/cancel-sell")] //do the sale by id
        public bool PutCancelSell(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                _quotesLogic.UpdateSale(quoteId, false);
                return true;
            }
            else
            {
                throw new ControllerExceptions("The ID couldn't be found ! { " + quoteId + " }");
            }
        }

        // DELETE: api/Quotes
        [HttpDelete("quotes/{quoteId}")] //delete by id
        public bool Delete(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                _quotesLogic.DeleteByID(quoteId);
                return true;
            }
            else
            {
                throw new ControllerExceptions("The ID couldn't be found ! { " + quoteId + " }");
            }
        }

    }
}
