using System;
using System.Collections.Generic;

namespace NogginAgenda.Data
{
	public class TimeSlot
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public IList<Talk> Talks { get; set; }

		public String ShortName
		{
			get
			{
				return String.Format("{0:t} - {1:t}", StartTime, EndTime);
			}
		}

		public String Title
		{
			get{
				return ShortName;
			}
		}
	}
}

