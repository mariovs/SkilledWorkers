using Profile.Api.Models;

namespace Profile.Api.Infrastructure
{
	public class MappingProfile : AutoMapper.Profile
	{
		public MappingProfile()
		{
			CreateMap<Models.Profile, ProfileRequestModel>().ReverseMap();
		}
	}
}
