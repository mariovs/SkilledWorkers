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
	public class ProfessionsController : ControllerBase
	{
		private readonly SkillsContext _context;

		public ProfessionsController(SkillsContext context)
		{
			_context = context;
		}

		// GET: api/Professions
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Profession>>> GetProfessions()
		{
			return await _context.Professions.ToListAsync();
		}

		// GET: api/Professions/5
		[HttpGet("{professionName}")]
		public async Task<ActionResult<Profession>> GetProfession(string professionName)
		{
			var profession = await _context.Professions.FindAsync(professionName);

			if (profession == null)
			{
				return NotFound();
			}

			return profession;
		}

		[HttpGet("{professionName}/skills")]
		public async Task<ActionResult<ProfessionSkillAvailableResponse>> GetSkillsAvailableForProfession(string professionName)
		{
			var profession = await _context.Professions.FindAsync(professionName);

			if (profession == null)
			{
				return NotFound();
			}

			var skillLvlsAvailableForProfession = new List<SkillLevel>();

			var allSkillLvls = await _context.SkillLevels.ToListAsync();
			var skillLvlNamesAvailable = await _context.ProfessionSkillAvailable.Where(p => p.ProfessionName == professionName).Select(s => s.SkillLevelName).ToListAsync();
			foreach (var skillLvlName in skillLvlNamesAvailable)
			{
				skillLvlsAvailableForProfession.Add(allSkillLvls.First(s => s.Name == skillLvlName));
			}

			return new ProfessionSkillAvailableResponse()
			{
				Profession = profession,
				SkillLevelsAvailable = skillLvlsAvailableForProfession
			};
		}

		[HttpPost("{professionName}/skills/{skillLvlName}")]
		public async Task<ActionResult<ProfessionSkillAvailableResponse>> AddSkillLvlForProfession(string professionName, string skillLvlName)
		{
			var profession = await _context.Professions.FindAsync(professionName);
			if (profession == null)
			{
				return NotFound();
			}

			var existingSkillLvl = await _context.SkillLevels.SingleOrDefaultAsync(s => s.Name == skillLvlName);
			if (existingSkillLvl == null)
			{
				return BadRequest();
			}


			var existingProfessionWithSkillLvl = await _context.ProfessionSkillAvailable.SingleOrDefaultAsync(s => s.ProfessionName == professionName && s.SkillLevelName == skillLvlName);
			if (existingProfessionWithSkillLvl == null)
			{
				await _context.ProfessionSkillAvailable.AddAsync(new ProfessionSkillsAvailable()
				{
					ProfessionName = professionName,
					SkillLevelName = skillLvlName
				});
			}

			await _context.SaveChangesAsync();
			return await GetSkillsAvailableForProfession(professionName);
		}


		[HttpDelete("{professionName}/{skillLvlName}")]
		public async Task<ActionResult<ProfessionSkillsAvailable>> DeleteSkillLvlForProfession(string professionName, string skillLvlName)
		{
			var existingProfessionWithSkillLvl = await _context.ProfessionSkillAvailable.SingleOrDefaultAsync(s => s.ProfessionName == professionName && s.SkillLevelName == skillLvlName);
			if (existingProfessionWithSkillLvl == null)
			{
				return NotFound();
			}

			_context.ProfessionSkillAvailable.Remove(existingProfessionWithSkillLvl);
			await _context.SaveChangesAsync();

			return existingProfessionWithSkillLvl;
		}

		// PUT: api/Professions/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPut("{professionName}")]
		public async Task<IActionResult> PutProfession(string professionName, Profession profession)
		{
			if (professionName != profession.Name)
			{
				return BadRequest();
			}

			_context.Entry(profession).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ProfessionExists(professionName))
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

		// POST: api/Professions
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for
		// more details see https://aka.ms/RazorPagesCRUD.
		[HttpPost]
		public async Task<ActionResult<Profession>> PostProfession(Profession profession)
		{
			_context.Professions.Add(profession);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (ProfessionExists(profession.Name))
				{
					return Conflict();
				}
				else
				{
					throw;
				}
			}

			return profession;
		}

		// DELETE: api/Professions/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Profession>> DeleteProfession(string id)
		{
			var profession = await _context.Professions.FindAsync(id);
			if (profession == null)
			{
				return NotFound();
			}

			_context.Professions.Remove(profession);
			await _context.SaveChangesAsync();

			return profession;
		}

		private bool ProfessionExists(string id)
		{
			return _context.Professions.Any(e => e.Name == id);
		}
	}
}
