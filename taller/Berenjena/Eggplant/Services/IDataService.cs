using Eggplant.Entity;
using Eggplant.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eggplant.Services
{
    public interface IDataService : IDisposable
    {
        TokenRepository Tokens { get; }
        int SaveChanges();
    }
}
