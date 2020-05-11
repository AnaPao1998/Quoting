using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Database
{
    public interface IDBManager
    {
        void InitDBContext();
        void SaveChanges();
    }
}
