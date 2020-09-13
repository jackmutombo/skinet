using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using API.Dtos;
using System.Linq;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrand;
        private readonly IGenericRepository<ProductType> _productType;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productType = productTypeRepo;
            _productBrand = productBrandRepo;
            _productRepo = productRepo;
        }

        // private readonly IProductRepository _repo;
        // public ProductsController(IProductRepository repo)
        // {
        //     _repo = repo;
        // }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productRepo.ListAsync(spec).ConfigureAwait(false);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepo.GetEntityWithSpec(spec).ConfigureAwait(false);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrands()
        {
            return Ok(await _productBrand.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductTypes()
        {
            return Ok(await _productType.ListAllAsync());
        }
    }
}