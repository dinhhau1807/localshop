using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Areas.Admin.ViewModels
{
    public class AddProductViewModel
    {
        public AddProductViewModel()
        {
            IsActive = true;
        }

        public bool IsActive { get; set; }
        public Product Product { get; set; }

        public string CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string StatusId { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
    }
}