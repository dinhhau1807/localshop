using AutoMapper;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Concretes
{
    public class HomePageRepository : IHomePageRepository
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;

        public HomePageRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public SpecialFeaturedDTO SpecialFeatureds
        {
            get
            {
                return _context.SpecialFeatureds.AsEnumerable().Select(sf => _mapper.Map<SpecialFeatured, SpecialFeaturedDTO>(sf)).FirstOrDefault();
            }
        }

        public bool SaveSpecialFeatureds(SpecialFeaturedDTO specialFeaturedDTO)
        {
            var specialFeatured = _context.SpecialFeatureds.FirstOrDefault(sf => sf.Id == specialFeaturedDTO.Id);

            if (specialFeatured == null)
            {
                specialFeatured = _mapper.Map<SpecialFeaturedDTO, SpecialFeatured>(specialFeaturedDTO);
                specialFeatured.Id = NewId.Next().ToString();

                _context.SpecialFeatureds.Add(specialFeatured);
                _context.SaveChanges();
            }
            else
            {
                specialFeatured = _mapper.Map(specialFeaturedDTO, specialFeatured);
                _context.SaveChanges();
            }

            return true;
        }
    }
}
