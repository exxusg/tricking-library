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
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TricksController(AppDbContext context)
        {
            _context = context;
        }

        // /api/tricks
        [HttpGet]
        public IEnumerable<Trick> All() => _context.Tricks.ToList();
        
        // /api/tricks/{id}
        [HttpGet("{id}")]
        public Trick Get(string id) => _context.Tricks.FirstOrDefault(trick => trick.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
        
        // /api/tricks/{id}/submissions - not implemented
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissions(string trickId) => 
            _context.Submissions
                .Where(submission => submission.TrickId.Equals(trickId, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        
        // /api/tricks
        [HttpPost]
        public async Task<Trick> Create([FromBody] Trick trick)
        {
            trick.Id = trick.Name.Replace(" ", "-").ToLowerInvariant();
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick;
        }
        // /api/tricks
        [HttpPut]
        public async Task<Trick> Update([FromBody] Trick trick)
        {
            if (string.IsNullOrEmpty(trick.Id))
            {
                return null;
            }
            
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick;
        }
        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var trick = _context.Tricks.FirstOrDefault(trick => trick.Id.Equals(id));
            trick.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}