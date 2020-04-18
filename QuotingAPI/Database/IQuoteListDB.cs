﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuotingAPI.Database.Models;

namespace QuotingAPI.Database
{
    interface IQuoteListDB
    {
        public List<Quote> GetAll();
    }
}
