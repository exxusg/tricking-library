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
        public Trick Get(int id) => _context.Tricks.FirstOrDefault(trick => trick.Id.Equals(id));
        
        // /api/tricks/{id}/submissions - not implemented
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissions(int trickId) => _context.Submissions.Where(submission => submission.TrickId.Equals(trickId)).ToList();
        
        // /api/tricks
        [HttpPost]
        public async Task<Trick> Create([FromBody] Trick trick)
        {
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick;
        }
        // /api/tricks
        [HttpPut]
        public async Task<Trick> Update([FromBody] Trick trick)
        {
            if (trick.Id == 0)
            {
                return null;
            }
            
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return trick;
        }
        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var trick = _context.Tricks.FirstOrDefault(trick => trick.Id.Equals(id));
            trick.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}