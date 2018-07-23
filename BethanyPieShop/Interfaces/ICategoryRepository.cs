using BethanyPieShop.Models;
using System.Collections.Generic;

namespace BethanyPieShop.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}
