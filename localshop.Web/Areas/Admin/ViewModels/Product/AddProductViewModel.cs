using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Areas.Admin.ViewModels
{
    public class AddProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}