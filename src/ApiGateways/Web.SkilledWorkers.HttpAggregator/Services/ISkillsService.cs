using System.Threading.Tasks;
using Web.SkilledWorkers.HttpAggregator.Models;

namespace Web.SkilledWorkers.HttpAggregator.Services
{
	public interface ISkillsService
	{
		Task<UserSkillsInfo> GetUserSkills(string userId);
	}
}
