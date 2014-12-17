using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data.Repositories
{
    public class UserRepository: GenericRepository<User>
    {
        public UserRepository(ThreeTrunksContext context) : base(context)
        {
        }
    }
}
