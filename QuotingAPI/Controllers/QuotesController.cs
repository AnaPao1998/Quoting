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
        public void Put(string quoteId, [FromBody] QuoteDTO updatedQuote)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("PUT BY ID =>\t | ID : " + quoteId + " | NAME : " + updatedQuote.QuoteName +
                 " | SellState : " + updatedQuote.IsSell + " | CLIENT CODE : " + updatedQuote.ClientCode);
                _quotesLogic.UpdateQuote(quoteId, updatedQuote);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }

        [HttpPut("quotes/{quoteId}/sell")] //do the sale by id
        public void PutSell(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("Sell => ID : " + quoteId);
                _quotesLogic.UpdateSale(quoteId, true);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }

        [HttpPut("quotes/{quoteId}/cancel-sell")] //do the sale by id
        public void PutCancelSell(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("CANCEL SELL => ID: " + quoteId);
                _quotesLogic.UpdateSale(quoteId, false);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }

        // DELETE: api/Quotes
        [HttpDelete("quotes/{quoteId}")] //delete by id
        public void Delete(string quoteId)
        {
            var exists = _quotesLogic.GetQuoteList().FirstOrDefault(q => q.QuoteID == quoteId);
            if (exists != null)
            {
                Console.WriteLine("DELETE BY ID =>\t | ID : " + quoteId);
                _quotesLogic.DeleteByID(quoteId);
            }
            else
            {
                Console.WriteLine("Error 404, not found");
            }
        }

    }
}
