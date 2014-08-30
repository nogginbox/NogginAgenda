using System;
using System.Collections.Generic;

namespace NogginAgenda.Data
{
	public class Speaker
	{
		public String Name { get; set; }

		public IList<Talk> Talks { get; set; }
	}
}