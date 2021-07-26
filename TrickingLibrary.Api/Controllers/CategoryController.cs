using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Category> All() => _context.Categories.ToList();
        
        [HttpGet("{id}")]
        public Category Get(string id) => 
            _context.Categories.FirstOrDefault(cat => 
                cat.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        
        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(string id) => 
            _context.TrickCategories
                .Include(tc => tc.Trick)
                .Where(tr => tr.CategoryId.Equals(id, StringComparison.CurrentCultureIgnoreCase))
                .Select(tr => tr.Trick)
                .ToList();
        
        // /api/tricks
        [HttpPost]
        public async Task<Category> Create([FromBody] Category category)
        {
            category.Id = category.Name.Replace(" ", "-").ToLowerInvariant();
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}