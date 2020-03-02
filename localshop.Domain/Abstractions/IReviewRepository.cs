using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Abstractions
{
    public interface IReviewRepository
    {
        IEnumerable<ReviewDTO> Reviews { get; }

        IEnumerable<ReviewDTO> GetReviews(string productId);

        bool Add(ReviewDTO reviewDTO);

        bool Delete(string userId, string productId);
    }
}
