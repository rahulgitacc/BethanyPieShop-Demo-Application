using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BethanyPieShop.Components
{
    public class AdminMenuItem : ViewComponent
    {
        public string DisplayValue { get; set; }
        public string ActionValue { get; set; }

        public IViewComponentResult Invoke()
        {
            var menuItems = new List<AdminMenuItem> {
                new AdminMenuItem()
                {
                    DisplayValue = "User management",
                    ActionValue = "UserManagement"

                },
                new AdminMenuItem()
                {
                    DisplayValue = "Role management",
                    ActionValue = "RoleManagement"
                }};

            return View(menuItems);
        }
    }
}
