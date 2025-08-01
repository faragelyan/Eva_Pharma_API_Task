using AutoMapper;
using BookShob.Application.Interfaces;
using BookShob.Domain.DTOs;
using BookShob.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookShob.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "ShortTerm")]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            // Same implementation as V1
            var products = await unitOfWork.ProductRepository.GetAllAsync();
            var dtos = mapper.Map<List<ProductDto>>(products);
            return Ok(dtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            // Same implementation as V1
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            var dto = mapper.Map<ProductDto>(product);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductDto dto)
        {
            // Same implementation as V1
            var product = mapper.Map<Product>(dto);
            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id, version = "2.0" }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, ProductDto dto)
        {
            // Same implementation as V1
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            mapper.Map(dto, product);
            await unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            // Same implementation as V1
            var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            await unitOfWork.ProductRepository.Delete(product);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}