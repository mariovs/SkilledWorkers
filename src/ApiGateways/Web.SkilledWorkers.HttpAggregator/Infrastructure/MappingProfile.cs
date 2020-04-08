using Web.SkilledWorkers.HttpAggregator.Models;

namespace Web.SkilledWorkers.HttpAggregator.Infrastructure
{
	public class MappingProfile : AutoMapper.Profile
	{
		public MappingProfile()
		{
			CreateMap<UserProfileInfo, UserProfileWithSkills>().ReverseMap();
		}
	}
}
