using BethanyPieShop.ApplicationDbContext;
using BethanyPieShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace BethanyPieShop.Interfaces
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public IEnumerable<Category> Categories => appDbContext.Categories.ToList();
    }
}
