using AutoMapper;
using localshop.Core.DTO;
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

            // Product Controller
            CreateMap<Product, ProductDTO>();

            // Category
            CreateMap<Category, CategoryDTO>();

            // Image
            CreateMap<Image, ImageDTO>();

            // Status
            CreateMap<Status, StatusDTO>();

            // Tag
            CreateMap<Tag, TagDTO>();
            #endregion

            #region DTO to Domain
            // Account Controller
            CreateMap<UpdateProfileDTO, ApplicationUser>();

            // Product Controller
            CreateMap<ProductDTO, Product>().ForMember(m => m.Images, opt => opt.Ignore());
            #endregion

            #region DTO to DTO
            // Product
            CreateMap<AddProductDTO, ProductDTO>().ForMember(m => m.Images, opt => opt.Ignore());
            CreateMap<EditProductDTO, ProductDTO>().ForMember(m => m.Images, opt => opt.Ignore());
            #endregion
        }

        public void ClientProfile()
        {

        }
    }
}