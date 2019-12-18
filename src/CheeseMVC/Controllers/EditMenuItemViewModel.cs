using CheeseMVC.Models;

namespace CheeseMVC.Controllers
{
    internal class EditMenuItemViewModel
    {
        private Cheese theCheese;

        public EditMenuItemViewModel(Cheese theCheese)
        {
            this.theCheese = theCheese;
        }
    }
}