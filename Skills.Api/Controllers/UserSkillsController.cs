﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Support;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skills.Api.Config;
using Skills.Api.DataAccess;
using Skills.Api.Models;

namespace Skills.Api.Controllers
{
	[Route("api/v1/skills/users")]
	[ApiController]
	public class UserSkillsController : ControllerBase
	{
		private readonly ILogger<UserSkillsController> _logger;
		private readonly SkillsContext _skillsContext;
		private readonly ApiLimitsConfig _apiLimitsConfig;


		public UserSkillsController(ILogger<UserSkillsController> logger, SkillsContext skillsContext, IOptions<ApiLimitsConfig> options)
		{
			_logger = logger;
			_skillsContext = skillsContext;
			_apiLimitsConfig = options.Value;
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<UserSkills>> GetUserSkils(string userId)
		{
			var user = await _skillsContext.UserSkills
				.Include(u => u.Skills)
				.ThenInclude(s => s.Profession)
				.Include(u => u.Skills)
				.ThenInclude(s => s.SkillLevel)
				.SingleOrDefaultAsync(u => u.UserId == userId);
			if (user == null)
			{
				return NotFound();
			}

			return user;
		}


		[HttpGet]
		public async Task<ActionResult<PaginatedItems<UserSkills>>> GetUserSkils([FromBody]List<string> userList, string professionName, string skillLvlName, int pageSize = 10, int pageNumber = 0)
		{
			if (pageSize > _apiLimitsConfig.MaxProfilesPerPage)
			{
				return BadRequest();
			}

			var isPorfessionAndSkillValid = await ValidateProfessionWithSkillLv(professionName, skillLvlName);
			if (!isPorfessionAndSkillValid)
			{
				return BadRequest();
			}

			var usersToSearch = userList.Skip(pageSize * pageNumber)
										.Take(pageSize);

			var userListFound = new List<UserSkills>();
			foreach (var userId in usersToSearch)
			{

				var user = await _skillsContext.UserSkills
					.Include(u => u.Skills)
					.ThenInclude(s => s.Profession)
					.Include(u => u.Skills)
					.ThenInclude(s => s.SkillLevel)
					.SingleOrDefaultAsync(u => u.UserId == userId);
				if (user != null)
				{
					userListFound.Add(user);
				}
			}

			return new PaginatedItems<UserSkills>(pageNumber, pageSize, userList.Count(), userListFound);
		}

		//get users with skills(users id [], profession, skilLvl)

		//add profession
		//add skill for profession
		//delete profession
		//updateProfession 
		//get all professions with skill levels
		//get all skills for a profession

		[HttpPost]
		[Route("{userId}")]
		public async Task<ActionResult<UserSkills>> AddSkillsToUserProfile(string userId, string professionName, string skillLvlName)
		{
			var exsitingProfession = await _skillsContext.Professions.FindAsync(professionName);
			if (exsitingProfession == null)
			{
				return BadRequest();
			}

			var existingSkillLvl = await _skillsContext.SkillLevels.FindAsync(skillLvlName);
			if (existingSkillLvl == null)
			{
				return BadRequest();
			}

			//is skill lvl available for profession
			var professionsWithSkills = await _skillsContext.ProfessionSkillAvailable.SingleOrDefaultAsync(p => p.SkillLevelName == skillLvlName && p.ProfessionName == professionName);
			if (professionsWithSkills == null)
			{
				return BadRequest();
			}

			var existingUser = await _skillsContext.UserSkills.Include(u => u.Skills)
				.ThenInclude(s => s.Profession)
				.Include(u => u.Skills)
				.ThenInclude(s => s.SkillLevel)
				.SingleOrDefaultAsync(u => u.UserId == userId);
			if (existingUser == null)
			{
				existingUser = new UserSkills()
				{
					UserId = userId
				};

				 await _skillsContext.UserSkills.AddAsync(existingUser);
			}

			//check if user has the skill
			var existingUserExistingSkill = existingUser.Skills.SingleOrDefault(s => s.Profession.Name == professionName);
			if (existingUserExistingSkill == null)
			{
				existingUser.Skills.Add(new Models.Skills()
				{
					Profession = exsitingProfession,
					SkillLevel = existingSkillLvl
				});
			}
			else
			{
				existingUserExistingSkill.SkillLevel = existingSkillLvl;
			}

			await _skillsContext.SaveChangesAsync();

			return await GetUserSkils(userId);
		}

		[HttpDelete("{userId}/professions/{professionName}")]
		public async Task<ActionResult<UserSkills>> DeleteSkillsToUserProfile(string userId, string professionName)
		{
			var exsitingProfession = await _skillsContext.Professions.FindAsync(professionName);
			if (exsitingProfession == null)
			{
				return BadRequest();
			}

			var existingUser = await _skillsContext.UserSkills
				.Include(u => u.Skills)
				.ThenInclude(s => s.Profession)
				.Include(u => u.Skills)
				.ThenInclude(s => s.SkillLevel)
				.SingleOrDefaultAsync(p => p.UserId == userId);

			if (existingUser == null)
			{
				return NotFound();
			}

			var existingSkillForUser = existingUser.Skills.FirstOrDefault(s => s.Profession.Name == professionName);
			if (existingSkillForUser == null)
			{
				return NotFound();
			}

			existingUser.Skills.Remove(existingSkillForUser);
			await _skillsContext.SaveChangesAsync();

			return await GetUserSkils(userId);
		}



		private async Task<bool> ValidateProfessionWithSkillLv(string professionName, string skillLvlName)
		{
			var exsitingProfession = await _skillsContext.Professions.FindAsync(professionName);
			if (exsitingProfession == null)
			{
				return false;
			}

			var existingSkillLvl = await _skillsContext.SkillLevels.FindAsync(skillLvlName);
			if (existingSkillLvl == null)
			{
				return false;
			}

			//is skill lvl available for profession
			var professionsWithSkills = await _skillsContext.ProfessionSkillAvailable.SingleOrDefaultAsync(p => p.SkillLevelName == skillLvlName && p.ProfessionName == professionName);
			if (professionsWithSkills == null)
			{
				return false;
			}

			return true;
		}

		private async Task PopulateWithData()
		{
			if(_skillsContext.Professions.Count() == 0)
			{
				_skillsContext.Professions.AddRange(new List<Profession>()
				{
					new Profession()
					{
						Name = "Chef",
						Description = "Ratatouile"
					},
					new Profession()
					{
						Name = "Bartender",
						Description = "Drinking problem"
					},
					new Profession()
					{
						Name = "Driver",
						Description = "F1 or limo, just easy"
					}
				});
				await _skillsContext.SaveChangesAsync();
			}

			if (_skillsContext.SkillLevels.Count() == 0)
			{
				_skillsContext.SkillLevels.AddRange(new List<SkillLevel>()
				{
					new SkillLevel()
					{
						Name = "Junior",
						Description = "Just a beginner"
					},
					new SkillLevel()
					{
						Name = "Medium",
						Description = "Couple of years exp"
					},
					new SkillLevel()
					{
						Name = "Pro",
						Description = "10 or more years"
					}
				});
				await _skillsContext.SaveChangesAsync();
			}

			//end

		}



	}
}