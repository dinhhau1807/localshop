using AutoMapper;
using localshop.Core.DTO.Admin;
using localshop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AdminProfile();
            ClientProfile();
        }

        public void AdminProfile()
        {
            #region Domain to DTO
            // Account Controller 
            CreateMap<ApplicationUser, UpdateProfileDTO>();
            #endregion

            #region DTO to Domain
            // Account Controller
            CreateMap<UpdateProfileDTO, ApplicationUser>();
            
            // Product Controller
            CreateMap<AddProductDTO, Product>().ForMember(m => m.Images, opt => opt.Ignore());
            CreateMap<EditProductDTO, Product>().ForMember(m => m.Images, opt => opt.Ignore());
            #endregion
        }

        public void ClientProfile()
        {

        }
    }
}