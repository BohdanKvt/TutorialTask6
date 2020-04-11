using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial6.Services
{
    public interface IDbService
    {
        bool ExistIndexNumber(string index);
        void SaveLogData(string data);
    }
}