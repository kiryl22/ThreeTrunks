using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data.Repositories
{
    public class ImageCategoryRepository : GenericRepository<ImageCategory>
    {
        public ImageCategoryRepository(ThreeTrunksContext context)
            : base(context)
        {

        }

        public List<ImageCategory> GetCategories(Expression<Func<ImageCategory, bool>> filter = null)
        {
            var categories = context.ImageCategories.Where(c => !c.IsDeleted);
            return filter != null ? categories.Where(filter).ToList() : categories.ToList();
        }

        public ImageCategory GetBasket()
        {
            var basket = context.ImageCategories.FirstOrDefault(c => c.IsBasket);

            if (basket == null)
            {
                basket = new ImageCategory()
                    {
                        IsBasket = true,
                        Name = "Корзина"
                    };

                base.Insert(basket);
                context.SaveChanges();
            }

            return basket;
        }
    }
}
