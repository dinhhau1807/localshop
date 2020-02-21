﻿using AutoMapper;
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
            CreateMap<ApplicationUser, UpdateProfileDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<Status, StatusDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<SpecialFeatured, SpecialFeaturedDTO>();
            #endregion


            #region DTO to Domain
            CreateMap<UpdateProfileDTO, ApplicationUser>();
            CreateMap<ProductDTO, Product>().ForMember(m => m.Images, opt => opt.Ignore());
            CreateMap<CategoryDTO, Category>();
            CreateMap<SpecialFeaturedDTO, SpecialFeatured>();
            #endregion


            #region DTO to DTO
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
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDetail, OrderDetailDTO>();
            #endregion


            #region DTO to Domain
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderDetailDTO, OrderDetail>();
            CreateMap<ContactDTO, Contact>();
            #endregion


            #region DTO to DTO
            CreateMap<ProductDTO, OrderDetailDTO>()
                .ForMember(od => od.Id, opt => opt.Ignore())
                .ForMember(od => od.ProductId, opt => opt.MapFrom(p => p.Id));
            #endregion
        }
    }
}