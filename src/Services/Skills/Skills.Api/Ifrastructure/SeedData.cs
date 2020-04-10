using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skills.Api.DataAccess;
using Skills.Api.Models;

namespace Skills.Api.Ifrastructure
{
	public class SeedData
	{
		public static void PopulateWithData(SkillsContext context)
		{
			if (context.Professions.Count() == 0)
			{
				context.Professions.AddRange(new List<Profession>()
				{
					new Profession()
					{
						Name = "Chef",
						Description = "He knows how to cook food"
					},
					new Profession()
					{
						Name = "Bartender",
						Description = "Name a drink and he will get you one"
					},
					new Profession()
					{
						Name = "Driver",
						Description = "Say any destination and you will be there"
					}
				});
				context.SaveChanges();
			}

			if (context.SkillLevels.Count() == 0)
			{
				context.SkillLevels.AddRange(new List<SkillLevel>()
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
				context.SaveChanges();
			}

			if (context.ProfessionSkillAvailable.Count() == 0)
			{
				context.ProfessionSkillAvailable.AddRange(new List<ProfessionSkillsAvailable>()
				{
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Chef",
						SkillLevelName = "Junior"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Chef",
						SkillLevelName = "Medium"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Chef",
						SkillLevelName = "Pro"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Driver",
						SkillLevelName = "Medium"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Driver",
						SkillLevelName = "Pro"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Bartender",
						SkillLevelName = "Medium"
					},
					new ProfessionSkillsAvailable()
					{
						ProfessionName = "Bartender",
						SkillLevelName = "Junior"
					}
				});

				context.SaveChanges();
			}

			if (context.UserSkills.Count() == 0)
			{
				context.UserSkills.AddRange(new List<UserSkills>()
				{
					new UserSkills()
					{
						UserId = "testUser1",
						Skills = new List<Models.Skills>()
						{
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Chef"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Pro")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Bartender"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Medium")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Driver"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Pro")
							}
						}
					},
					new UserSkills()
					{
						UserId = "testUser2",
						Skills = new List<Models.Skills>()
						{
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Chef"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Junior")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Bartender"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Medium")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Driver"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Pro")
							}
						}
					},
					new UserSkills()
					{
						UserId = "testUser3",
						Skills = new List<Models.Skills>()
						{
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Bartender"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Junior")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Driver"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Medium")
							}
						}
					},
					new UserSkills()
					{
						UserId = "testUser4",
						Skills = new List<Models.Skills>()
						{
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Chef"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Pro")
							},
							new Models.Skills()
							{
								Profession = context.Professions.First(p => p.Name == "Driver"),
								SkillLevel = context.SkillLevels.First(s => s.Name == "Medium")
							}
						}
					}
				});

				context.SaveChanges();
			}
		}
	}
}
