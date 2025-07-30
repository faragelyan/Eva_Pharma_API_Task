using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookShob.Domain.DTOs;
using BookShob.Domain.Entities;
namespace BookShob.Application.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
                CreateMap<Category,CategoryDto>().ReverseMap();
        }
    }
}
