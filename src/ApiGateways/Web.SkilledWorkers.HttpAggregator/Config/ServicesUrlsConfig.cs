using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.SkilledWorkers.HttpAggregator.Config
{
	public class ServicesUrlsConfig
	{
		public class ProfileOperations
		{
			public static string GetProfile(string userId) => $"/api/v1/profiles/{userId}";

			public static string CreateProfile() => $"/api/v1/profiles";

			public static string GetProfiles(int pageSize = 10, int pageNumber = 0) => $"/api/v1/profiles?pageNumber={pageNumber}&pageSize={pageSize}";

			public static string GetProfiles(string address, double radius, int pageSize = 10, int pageNumber = 0) => $"/api/v1/profiles/search?address={address}&radius={radius}&pageNumber={pageNumber}&pageSize={pageSize}";
		}

		public class SkillsOperations
		{
			public static string GetSkillsForUser(string userId) => $"/api/v1/skills/users/{userId}";

			public static string GetSkillsForUser(string userId, string professionName, string skillLevelName) => $"/api/v1/skills/users/{userId}/search?professionName={professionName}&skillLevelName={skillLevelName}";

			public static string GetSkillsForUserIds(string baseUrl, string[] usersId, string professionName, string skillLevelName, int pageSize, int pageNumber)
			{
				var paramDictionary = new Dictionary<string, string>();
				paramDictionary.Add("professionName", professionName);
				paramDictionary.Add("skillLevelName", skillLevelName);
				paramDictionary.Add("pageSize", pageSize.ToString());
				paramDictionary.Add("pageNumber", pageNumber.ToString());
				paramDictionary.Add("userList", $"{string.Join(',', usersId).ToString()}");

				return new Uri(QueryHelpers.AddQueryString(baseUrl, paramDictionary)).ToString();
			}
		}

		public string ProfilesApi { get; set; }
		public string ProfileApiVersion { get; set; }

		public string SkillsApi { get; set; }
	}
}
