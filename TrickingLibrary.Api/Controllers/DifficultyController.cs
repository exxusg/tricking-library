using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/difficulties")]
    public class DifficultyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DifficultyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Difficulty> All() => _context.Difficulties.ToList();
        
        [HttpGet("{id}")]
        public Difficulty Get(string id) => 
            _context.Difficulties.FirstOrDefault(dif => 
                dif.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        
        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListDifficultyTricks(string id) => 
            _context.Tricks
                .Where(tr => tr.Difficulty.Equals(id, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        
        // /api/tricks
        [HttpPost]
        public async Task<Difficulty> Create([FromBody] Difficulty difficulty)
        {
            difficulty.Id = difficulty.Name.Replace(" ", "-").ToLowerInvariant();
            _context.Add(difficulty);
            await _context.SaveChangesAsync();
            return difficulty;
        }
    }
}