﻿using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddEditCheeseViewModel : AddCheeseViewModel
    {
        public int cheeseId { get; set; }

        public AddEditCheeseViewModel()
        { }

        public AddEditCheeseViewModel(IEnumerable<CheeseCategory> categories) : base(categories)
        {

        }
    }


}
