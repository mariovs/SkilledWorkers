using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skills.Api.DataAccess;
using Skills.Api.Models;

namespace Skills.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SkillLevelsController : ControllerBase
    {
        private readonly SkillsContext _context;

        public SkillLevelsController(SkillsContext context)
        {
            _context = context;
        }

        // GET: api/SkillLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillLevel>>> GetSkillLevels()
        {
            return await _context.SkillLevels.ToListAsync();
        }

        // GET: api/SkillLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SkillLevel>> GetSkillLevel(string id)
        {
            var skillLevel = await _context.SkillLevels.FindAsync(id);

            if (skillLevel == null)
            {
                return NotFound();
            }

            return skillLevel;
        }

        // PUT: api/SkillLevels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkillLevel(string id, SkillLevel skillLevel)
        {
            if (id != skillLevel.Name)
            {
                return BadRequest();
            }

            _context.Entry(skillLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillLevelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SkillLevels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<SkillLevel>> PostSkillLevel(SkillLevel skillLevel)
        {
            _context.SkillLevels.Add(skillLevel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SkillLevelExists(skillLevel.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return skillLevel;
        }

        // DELETE: api/SkillLevels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SkillLevel>> DeleteSkillLevel(string id)
        {
            var skillLevel = await _context.SkillLevels.FindAsync(id);
            if (skillLevel == null)
            {
                return NotFound();
            }

            _context.SkillLevels.Remove(skillLevel);
            await _context.SaveChangesAsync();

            return skillLevel;
        }

        private bool SkillLevelExists(string id)
        {
            return _context.SkillLevels.Any(e => e.Name == id);
        }
    }
}
