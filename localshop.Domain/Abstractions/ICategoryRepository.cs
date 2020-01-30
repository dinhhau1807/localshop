using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Abstractions
{
    public interface ICategoryRepository : IDisposable
    {
        IQueryable<Category> Categories { get; }
    }
}
