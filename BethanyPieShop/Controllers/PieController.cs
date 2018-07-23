using BethanyPieShop.Interfaces;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanyPieShop.Controllers
{
    public class PieController : Controller
    {
        private ICategoryRepository categoryRepository;
        private IPieRepository pieRepository;

        public PieController(ICategoryRepository categoryRepository, IPieRepository pieRepository)
        {
            this.categoryRepository = categoryRepository;
            this.pieRepository = pieRepository;
        }

        public IActionResult List()
        {
            var piesListViewModel = new PiesListViewModel();
            piesListViewModel.Pies = pieRepository.Pies;
            piesListViewModel.CurrentCategory = "Cheese cakes";
            return View(piesListViewModel);
        }
    }
}