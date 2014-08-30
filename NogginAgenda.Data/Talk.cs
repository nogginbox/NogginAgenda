using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NogginAgenda.Data
{
	public class Talk
	{
		public String Title { get; set; }
		public String Description { get; set; }

		public Speaker Speaker { get; set; }
	}
}