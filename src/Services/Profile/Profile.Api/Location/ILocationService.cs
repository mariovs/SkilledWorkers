using System.Threading.Tasks;
using Profile.Api.Models;

namespace Profile.Api.Location
{
	public interface ILocationService
	{
		Task<AddressCoordinates> GetCoordinates(string address);
	}
}
