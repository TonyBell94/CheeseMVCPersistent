using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int cheeseID { get; set; }
        public int menuID { get; set; }

        public Menu Menu { get; set; }

        public List<SelectListItem> Cheeses { get; set; }

        public AddMenuItemViewModel() { }


        public AddMenuItemViewModel(Menu aMenu, IEnumerable<Cheese> theSelectListCheeses)
        {
            Menu = aMenu;

            Cheeses = new List<SelectListItem>();

            foreach (var chester in theSelectListCheeses)
            {
                Cheeses.Add(new SelectListItem
                {
                    Value = chester.ID.ToString(),
                    Text = chester.Name
                });
            }

        }
    }
}

