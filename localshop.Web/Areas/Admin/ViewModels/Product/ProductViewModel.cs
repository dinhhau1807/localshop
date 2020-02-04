using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            IsActive = true;
        }

        public bool IsActive { get; set; }
        public ProductDTO Product { get; set; }

        public string Images { get; set; }

        public string CategoryId { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }

        public string StatusId { get; set; }
        public IEnumerable<StatusDTO> Statuses { get; set; }
    }
}