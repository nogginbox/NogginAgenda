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

		public TimeSlot TimeSlot { get; set; }

		public String TimeAndRoom
		{
			get {return String.Format ("{0} - room {1}", TimeSlot.ShortName, "TBA");} 
		}
	}
}