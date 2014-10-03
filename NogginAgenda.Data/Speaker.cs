using System;
using System.Collections.Generic;

namespace NogginAgenda.Data
{
	public class Speaker
	{
		public String Name { get; set; }

		public String PictureUrl { get; set; }

        public String TwitterUrl { get; set; }

		public String WebsiteUrl { get; set; }

		public IList<Talk> Talks { get; set; }
	}
}