using AutoMapper;
using BookShob.Application.Interfaces;
using BookShob.Domain.DTOs;
using BookShob.Domain.Entities;
using BookShob.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShob.API.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet("paginated")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationParams pagination)
        {
            var categories = await unitOfWork.CategoryRepository
                .GetPaginatedAsync(pagination.PageNumber, pagination.ValidPageSize);

            var totalCount = await unitOfWork.CategoryRepository.CountAsync();

            var categoryDtos = mapper.Map<List<CategoryDto>>(categories);

            var response = new
            {
                TotalCount = totalCount,
                pagination.PageNumber,
                PageSize = pagination.ValidPageSize,
                Data = categoryDtos
            };

            return Ok(response);
        }


        [HttpGet]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<ActionResult<List<CategoryDto>>> GetAll()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            var dtos = mapper.Map<List<CategoryDto>>(categories);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        [ResponseCache(CacheProfileName = "ShortTerm")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            var dto = mapper.Map<CategoryDto>(category);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryDto dto)
        {
            var category = mapper.Map<Category>(dto);
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id,CategoryDto dto)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            mapper.Map(dto, category);
            await unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            await unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.SaveAsync();
            return NoContent();

        }

    }
}
