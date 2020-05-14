using BackingServices.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Services
{
    public class PricingBookBackingService : IPricingBookBackingService
    {
        private readonly IConfiguration _configuration;
        public PricingBookBackingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<ProductBsDTO>> GetAllProduct()
        {
            string msPath = _configuration.GetSection("Microservices").GetSection("PricingBooks").Value;

            try
            {
                // Creating HTTP Client
                HttpClient productMS = new HttpClient();

                // Executing an ASYNC HTTP Method could be: Get, Post, Put, Delete
                // In this case is a GET
                // HttpContent content = new 
                HttpResponseMessage response = await productMS.GetAsync($"{msPath}/pricing-books/active");
                // HttpResponseMessage response = await productMS.GetAsync($"{msPath}/campaigns/Campaigns-001");
                //HttpResponseMessage response = await productMS.GetAsync($"{msPath}/products");

                int statusCode = (int)response.StatusCode;
                if (statusCode == 200) // OK
                {
                    // Read ASYNC response from HTTPResponse 
                    String jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize response
                    PricingBookBsDTO pricingBook = JsonConvert.DeserializeObject<PricingBookBsDTO>(jsonResponse);

                    return pricingBook.ProductPrices;
                }
                else
                {
                    // something wrong happens!
                    // Add a new Exception for Backing Service
                    throw new BackingServiceException("BS throws the error: " + statusCode);
                }
            }
            catch (Exception ex)
            {
                throw new BackingServiceException("Connection with PricingBooks is not working! " + msPath);
            }
        }
    }
}
