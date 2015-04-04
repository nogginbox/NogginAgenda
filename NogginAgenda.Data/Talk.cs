using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NogginAgenda.Data
{
	public class Talk
	{
		public String Title { get; set; }

        public String Subtitle { get; set; }

		public String Description { get; set; }

        public String Room { get; set; }

        public String RoomLongName { get { return String.Format ("Room {0}", Room);} }

		public Speaker Speaker { get; set; }

		public TimeSlot TimeSlot { get; set; }

		public String TimeAndRoom
		{
			get {return String.Format ("{0} - Room {1}", TimeSlot.ShortName, Room);} 
		}
	}
}