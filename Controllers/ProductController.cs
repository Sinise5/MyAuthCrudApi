using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAuthCrudApi.Models;
using MyAuthCrudApi.Repositories;
using System.Security.Claims;

namespace MyAuthCrudApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Semua endpoint butuh autentikasi
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepo;

    public ProductController(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    // Mendapatkan semua produk
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepo.GetAllAsync();
        return Ok(products);
    }

    // Mendapatkan produk berdasarkan ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    // Menambahkan produk baru
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (product == null) return BadRequest();
        await _productRepo.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // Mengupdate produk berdasarkan ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (product == null || id != product.Id) return BadRequest();
        
        var existingProduct = await _productRepo.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();

        await _productRepo.UpdateAsync(product);
        return NoContent();
    }

    // Menghapus produk berdasarkan ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingProduct = await _productRepo.GetByIdAsync(id);
        if (existingProduct == null) return NotFound();

        await _productRepo.DeleteAsync(id);
        return NoContent();
    }
}
