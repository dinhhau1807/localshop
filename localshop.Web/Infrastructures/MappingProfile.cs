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
            // Account 
            CreateMap<ApplicationUser, UpdateProfileDTO>();

            // Product 
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
            // Account 
            CreateMap<UpdateProfileDTO, ApplicationUser>();

            // Product 
            CreateMap<ProductDTO, Product>().ForMember(m => m.Images, opt => opt.Ignore());

            // Category
            CreateMap<CategoryDTO, Category>();
            #endregion


            #region DTO to DTO
            // Product
            CreateMap<AddProductDTO, ProductDTO>().ForMember(m => m.Images, opt => opt.Ignore());
            CreateMap<EditProductDTO, ProductDTO>().ForMember(m => m.Images, opt => opt.Ignore());
            #endregion
        }

        public void ClientProfile()
        {
            #region Domain to DTO
            CreateMap<ApplicationUser, OrderDTO>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.UserId, opt => opt.MapFrom(u => u.Id));

            #endregion


            #region DTO to Domain
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderDetailDTO, OrderDetail>();
            #endregion


            #region DTO to DTO
            CreateMap<ProductDTO, OrderDetailDTO>()
                .ForMember(od => od.Id, opt => opt.Ignore())
                .ForMember(od => od.ProductId, opt => opt.MapFrom(p => p.Id));
            #endregion
        }
    }
}