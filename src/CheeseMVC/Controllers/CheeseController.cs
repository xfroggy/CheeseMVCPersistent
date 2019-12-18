using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            context = dbContext;  
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include( c => c.Category).ToList()
;            //List<Cheese> cheeses = context.Cheeses.ToList();

            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = 
                new AddCheeseViewModel(context.Categories.ToList());
            
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                
                // Add the new cheese to my existing cheeses
                CheeseCategory newCheeseCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

                var cheeseName = addCheeseViewModel.Name;

                IList<Cheese> existingCheeses = context.Cheeses
                    .Where(c => c.Name.ToLower() == cheeseName.ToLower()).ToList();
                if (existingCheeses.Count == 0)
                {
                    Cheese newCheese = new Cheese
                    {
                        Name = addCheeseViewModel.Name,
                        Description = addCheeseViewModel.Description,
                        Category = newCheeseCategory
                    };

                    context.Cheeses.Add(newCheese);
                    context.SaveChanges();
                }
                return Redirect("/Cheese");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Edit(int cheeseId)
        {
            AddEditCheeseViewModel addEditCheeseViewModel =
                new AddEditCheeseViewModel(context.Categories.ToList());
            Cheese thisCheese = context.Cheeses.Single(c => c.ID == cheeseId);
            addEditCheeseViewModel.Name = thisCheese.Name;
            addEditCheeseViewModel.Description = thisCheese.Description;
            addEditCheeseViewModel.CategoryID = thisCheese.CategoryID;
            addEditCheeseViewModel.cheeseId = cheeseId;

            return View(addEditCheeseViewModel);

        }
        [HttpPost]
        public IActionResult Edit(AddEditCheeseViewModel addEditCheeseViewModel)
        {

            if (ModelState.IsValid)
            {

               var cheeseName = addEditCheeseViewModel.Name;

                IList<Cheese> existingCheeses = context.Cheeses
                    .Where(c => c.Name.ToLower() == cheeseName.ToLower()).ToList();
                if (existingCheeses.Count == 0)
                {
                    // Create a new cheese from ViewModel binding and then since it has the ID already, update existing               

                    Cheese thisCheese = new Cheese()
                    {

                        ID = addEditCheeseViewModel.cheeseId,
                        Name = addEditCheeseViewModel.Name,
                        Description = addEditCheeseViewModel.Description,
                        CategoryID = addEditCheeseViewModel.CategoryID
                    };


                    context.Cheeses.Update(thisCheese);
                    context.SaveChanges();

                }
                return Redirect("/Cheese");                
            }

            return View(addEditCheeseViewModel);


        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        //Controller/Action/Id - default routing
        public IActionResult Category(int id)
        {
            if (id == 0)
            {
                return Redirect("/Category");
            }

            CheeseCategory theCategory = context.Categories
                .Include(cat => cat.Cheeses)
                .Single(cat => cat.ID == id);

            /* alternative
             
             IList<Cheese> theCheeses = context.Cheeses
                .Include(c => c.Category)
                .Where(c => c.CategoryID == id)
                .ToList();
             
             */
            ViewBag.title = "Cheeses in category: " + theCategory.Name;
            return View("Index", theCategory.Cheeses);
        }
    
    }
}   
