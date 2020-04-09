using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Support;
using Web.SkilledWorkers.HttpAggregator.Models;

namespace Web.SkilledWorkers.HttpAggregator.Services
{
	public interface ISkillsService
	{
		Task<UserSkillsInfo> GetUserSkills(string userId);

		Task<UserSkillsInfo> GetUserWithSkills(string userId, string professionName, string skillLvlName);

		Task<PaginatedItems<UserSkillsInfo>> GetUsersSkills(List<string> userIdList, string professionName, string skillLvlName, int pageNumber, int pageSize);
	}
}
