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
        IEnumerable<ContactDTO> Contacts { get; }

        ContactDTO Save(ContactDTO contactDTO);

        void SetRead(int contactId);
    }
}
