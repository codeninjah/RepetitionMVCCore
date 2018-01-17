using Microsoft.AspNetCore.Identity;
using Repetition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repetition.Data
{
    public class DbSeeder
    {
		//LAGT TILL INJECTION SAMT IF SATSER
		public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			if (!context.Roles.Any())
			{
				var admin = new IdentityRole { Name = "Admin" };
				var result = roleManager.CreateAsync(admin);
			}

			if (!context.Users.Any())
			{
				var user = new ApplicationUser { UserName = "student@test.com", Email = "student@test.com" };
				var result = userManager.CreateAsync(user, "Test-123").Result;
				var roleResult = userManager.AddToRoleAsync(user, "Admin").Result;
			}

			var artist = new Artist[]
			{
				new Artist{Name="Eminem" },
				new Artist{Name = "M.G" },
				new Artist {Name = "Dr Dre" },
			};

			foreach (Artist a in artist)
			{
				context.Artist.Add(a);
			}

			//if (context.Artist.Any())
			//{
			//	return;
			//}

			var song = new Song[]
		   {
				new Song{Name="Mostafa is hot", ArtistID =1},
				new Song{Name="Gentrit is hot", ArtistID =1},
				new Song {Name = "Tragic Endings", ArtistID =2 },
		   };

			//foreach (Song s in song)
			//{
			//	context.Songs.Add(s);
			//}
			context.Songs.AddRange(song);
			context.SaveChanges();

			//if (context.Songs.Any())
			//{
			//	return;
			//}

		}
	}
}
