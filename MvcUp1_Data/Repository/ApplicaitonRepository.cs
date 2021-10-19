using MvcUp1_Data.Repository.IRepository;
using MvcUp1_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcUp1_Data.Repository
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public void Update(Application application)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == application.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = application.Name;
            }
        }
    }
}
