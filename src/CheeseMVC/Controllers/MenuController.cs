using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<CheeseMenu> because = context.CheeseMenus.ToList();
            return View(because);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu()
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }
        [HttpGet]
        public IActionResult ViewMenu(int id)
        {

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            Menu daMenu = context.Menus.Single(c => c.ID == id);


            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
            {
                Menu = daMenu,
                Items = items
            };

            ViewBag.Title = viewMenuViewModel.Menu.Name;

            return View(viewMenuViewModel);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu daMenu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();
            return View(new AddMenuItemViewModel(daMenu, cheeses));
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {

                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu();
                    newCheeseMenu.MenuID = addMenuItemViewModel.menuID;
                    newCheeseMenu.CheeseID = addMenuItemViewModel.cheeseID;

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();

                    return Redirect(string.Format("/Menu/ViewMenu/{0}", newCheeseMenu.MenuID));

                }

                return Redirect("/Menu");


            }

            return View(addMenuItemViewModel);

        }
    }
}
