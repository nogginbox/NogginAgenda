using System;
using System.Collections.Generic;

namespace NogginAgenda.Data
{
	public class TimeSlot
	{
		public TimeSlot()
		{
			Talks = new List<Talk> ();
		}

		public TimeSlot(String val) : this()
		{
			var bits = val.Split (',');
			if (bits.Length != 2)
				throw new FormatException ("TimeSlot format wrong: " + val);

			try{
				StartTime = DateTime.Parse(bits[0]);
				EndTime = DateTime.Parse(bits[1]);
			}
			catch(Exception e) {
				throw new FormatException ("TimeSlot time format wrong: " + val, e);
			}
		}


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

		public bool Equals(TimeSlot t)
		{
			// If parameter is null return false:
			if ((object)t == null)
			{
				return false;
			}
				
			return (StartTime == t.StartTime) && (EndTime == t.EndTime);
		}
	}
}

