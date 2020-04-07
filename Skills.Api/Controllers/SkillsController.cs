using Microsoft.AspNetCore.Mvc;

namespace Skills.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SkillsController : ControllerBase
	{

		//[HttpPost]
		//[Route("{userId}/skills")]
		//public async Task<ActionResult<Models.Profile>> AddSkillsToProfile(string userId, [FromBody] List<SkillViewModel> skills)
		//{
		//	if (skills.Count > maxSkillsPerProfile)
		//	{
		//		return BadRequest();
		//	}

		//	//validate skill pairs
		//	foreach (var skill in skills)
		//	{
		//		var skillInDb = _professionalsContext.ProfessionSkillAvailable.SingleOrDefaultAsync(p => p.ProfessionName == skill.ProfessionName && p.SkillLevelName == skill.SkillLevelName);
		//		if (skillInDb == null)
		//		{
		//			return BadRequest();
		//		}
		//	}

		//	var existingUser = await _professionalsContext.Profiles.SingleOrDefaultAsync(p => p.Id == userId);
		//	if (existingUser != null)
		//	{
		//		return BadRequest();
		//	}

		//	return null;
		//}

	}
}