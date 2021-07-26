using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Api.ViewModels;
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
        public IEnumerable<object> All() => _context.Tricks.Select(TrickViewModel.Default).ToList();
        
        // /api/tricks/{id}
        [HttpGet("{id}")]
        public object Get(string id) => 
            _context.Tricks
                .Where(trick => trick.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase))
                .Select(TrickViewModel.Default)
                .FirstOrDefault();
        
        // /api/tricks/{id}/submissions - not implemented
        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> ListSubmissions(string trickId) => 
            _context.Submissions
                .Include(s => s.Video)
                .Where(submission => submission.TrickId.Equals(trickId, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        
        // /api/tricks
        [HttpPost]
        public async Task<object> Create([FromBody] TrickForm trickForm)
        {
            var trick = new Trick()
            {
                Id = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(tr => new TrickCategory { CategoryId = tr }).ToList()
            };
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return TrickViewModel.Default.Compile().Invoke(trick);
        }
        // /api/tricks
        [HttpPut]
        public async Task<object> Update([FromBody] Trick trick)
        {
            if (string.IsNullOrEmpty(trick.Id))
            {
                return null;
            }
            
            _context.Add(trick);
            await _context.SaveChangesAsync();
            return TrickViewModel.Default.Compile().Invoke(trick);
        }
        // /api/tricks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var trick = _context.Tricks.FirstOrDefault(t => t.Id.Equals(id));
            if (trick != null) 
                trick.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}