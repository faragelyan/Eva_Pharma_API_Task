using AutoMapper;
using BookShob.Application.Interfaces;
using BookShob.Domain.DTOs;
using BookShob.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShob.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize] // ✅ Require authentication for all methods by default
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // 👥 Any authenticated user (User, Admin) can view products
        [HttpGet]
        [AllowAnonymous] // or [Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "ShortTerm")]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            var dtos = mapper.Map<List<ProductDto>>(products);
            return Ok(dtos);
        }

        // 👥 Any authenticated user can view product details
        [HttpGet("{id:int}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var dto = mapper.Map<ProductDto>(product);
            return Ok(dto);
        }

        // 🛠 Only Admin can create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ProductDto dto)
        {
            var product = mapper.Map<Product>(dto);
            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);
        }

        // 🛠 Only Admin can update
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, ProductDto dto)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            mapper.Map(dto, product);
            await unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveAsync();
            return NoContent();
        }

        // 🛠 Only Admin can delete
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            await unitOfWork.ProductRepository.Delete(product);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
