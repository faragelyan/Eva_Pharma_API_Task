using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookShob.Domain.Entities;
using BookShob.Domain.DTOs;
namespace BookShob.Application.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
                CreateMap<Product,ProductDto>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore()); 
        }
    }
}
