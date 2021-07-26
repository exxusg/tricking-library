using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.BackgroundServices;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        // /api/submissions
        [HttpGet]
        public IEnumerable<Submission> All() => _context.Submissions.Where(x => x.VideoProcessed).ToList();
        
        // /api/submissions/{id}
        [HttpGet("{id}")]
        public Submission Get(int id) => _context.Submissions.FirstOrDefault(submission => submission.Id.Equals(id));

        // /api/submissions
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] SubmissionForm submissionForm, 
            [FromServices] Channel<EditVideoMessage> channel,
            [FromServices] VideoManager videoManager)
        {
            if (!videoManager.TemporaryFileExists(submissionForm.Video))
                return BadRequest();

            var submission = new Submission
            {
                TrickId = submissionForm.TrickId,
                Description = submissionForm.Description,
                VideoProcessed = false,
            };
            submission.VideoProcessed = false;
            _context.Add(submission);
            await _context.SaveChangesAsync();
            await channel.Writer.WriteAsync(new EditVideoMessage
            {
                SubmissionId = submission.Id,
                Input = submissionForm.Video,
                Output = $"converted_{DateTime.Now.Ticks}"
            });
            
            return Ok(submission);
        }
        // /api/submissions
        [HttpPut]
        public async Task<Submission> Update([FromBody] Submission submission)
        {
            if (submission.Id == 0)
            {
                return null;
            }
            
            _context.Add(submission);
            await _context.SaveChangesAsync();
            return submission;
        }
        // /api/submissions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var submission = _context.Submissions.FirstOrDefault(sub => sub.Id.Equals(id));
            if (submission == null)
            {
                return NotFound();
            }
            
            submission.Deleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}