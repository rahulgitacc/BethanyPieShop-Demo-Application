using BethanyPieShop.Interfaces;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace BethanyPieShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    [Authorize(Policy = "DeletePie")]
    public class PieManagementController : Controller
    {
        private readonly IPieRepository pieRepository;
        private readonly ICategoryRepository categoryRepository;

        public PieManagementController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            this.pieRepository = pieRepository;
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var pies = pieRepository.Pies.OrderBy(p => p.PieId);
            return View();
        }

        public IActionResult AddPie()
        {
            var categories = categoryRepository.Categories;
            var pieEditViewModel = new PieEditViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                CategoryId = categories.FirstOrDefault().CategoryId
            };
            return View(pieEditViewModel);
        }

        [HttpPost]
        public IActionResult AddPie(PieEditViewModel pieEditViewModel)
        {
            //Basic validation
            if (ModelState.IsValid)
            {
                pieRepository.CreatePie(pieEditViewModel.Pie);
                return RedirectToAction("Index");
            }
            return View(pieEditViewModel);
        }

        public IActionResult EditPie(int pieId)
        {
            var categories = categoryRepository.Categories;

            var pie = pieRepository.Pies.FirstOrDefault(p => p.PieId == pieId);

            var pieEditViewModel = new PieEditViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                Pie = pie,
                CategoryId = pie.CategoryId
            };

            var item = pieEditViewModel.Categories.FirstOrDefault(c => c.Value == pie.CategoryId.ToString());
            item.Selected = true;

            return View(pieEditViewModel);
        }

        [HttpPost]
        public IActionResult EditPie(PieEditViewModel pieEditViewModel)
        {
            pieEditViewModel.Pie.CategoryId = pieEditViewModel.CategoryId;

            if (ModelState.IsValid)
            {
                pieRepository.UpdatePie(pieEditViewModel.Pie);
                return RedirectToAction("Index");
            }
            return View(pieEditViewModel);
        }

        [HttpPost]
        public IActionResult DeletePie(string pieId)
        {
            return View();
        }
    }
}