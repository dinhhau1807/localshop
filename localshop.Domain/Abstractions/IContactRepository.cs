using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Abstractions
{
    public interface IContactRepository
    {
        ContactDTO Save(ContactDTO contactDTO);
    }
}
