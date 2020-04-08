using System.Threading.Tasks;
using Api.Support;
using Web.SkilledWorkers.HttpAggregator.Models;

namespace Web.SkilledWorkers.HttpAggregator.Services
{
	public interface IProfileService
	{
		Task<UserProfileInfo> GetUserById(string userId);

		Task<UserProfileInfo> CreateUser(UserProfileInfoRequest userProfileRequest);

		Task<PaginatedItems<UserProfileInfo>> GetUserProfileInfoList(int pageNumber, int pageSize);

		Task<PaginatedItems<UserProfileInfo>> GetUserProfileInfoList(string address, double radius);
	}
}
