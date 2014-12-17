using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data.Repositories
{
    public class ImageRepository: GenericRepository<Image>
    {
        public ImageRepository(ThreeTrunksContext context) : base(context)
        {
        }
    }
}
