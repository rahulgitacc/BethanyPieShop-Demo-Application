using BethanyPieShop.Models;
using System.Collections.Generic;

namespace BethanyPieShop.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Pie> PiesOfTheWeek { get; set; }
    }
}
