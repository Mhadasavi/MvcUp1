using MvcUp1_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcUp1_Data.Repository.IRepository
{
    public interface IApplicationRepository : IRepository<Application>
    {
        void Update(Application application);
    }
}
