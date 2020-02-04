using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Core.DTO
{
    public class CategoryDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }
    }
}
