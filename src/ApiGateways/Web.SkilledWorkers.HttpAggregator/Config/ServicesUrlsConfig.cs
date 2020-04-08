namespace Web.SkilledWorkers.HttpAggregator.Config
{
	public class ServicesUrlsConfig
	{
		public class ProfileOperations
		{
			public static string GetProfile(string userId) => $"/api/v1/profiles/{userId}";

			public static string CreateProfile() => $"/api/v1/profiles";

			public static string GetProfiles(int pageSize = 10, int pageNumber = 0) => $"/api/v1/profiles?pageNumber={pageNumber}&pageSize={pageSize}";
		}

		public class SkillsOperations
		{
			public static string GetSkillsForUser(string userId) => $"/api/v1/skills/users/{userId}";
		}

		public string ProfilesApi { get; set; }
		public string ProfileApiVersion { get; set; }

		public string SkillsApi { get; set; }
	}
}
