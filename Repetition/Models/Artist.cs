using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repetition.Models
{
	public class Artist
	{
		public int ArtistID { get; set; }
		public string Name { get; set; }
		public List<Song> Songs { get; set; }
	}
}
