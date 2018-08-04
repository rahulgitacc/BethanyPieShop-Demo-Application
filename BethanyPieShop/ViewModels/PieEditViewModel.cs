using BethanyPieShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BethanyPieShop.ViewModels
{
    public class PieEditViewModel
    {
        public Pie Pie { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
