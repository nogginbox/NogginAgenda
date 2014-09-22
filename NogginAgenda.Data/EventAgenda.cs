using System;
using System.Collections.Generic;
using System.Linq;

namespace NogginAgenda.Data
{
	public class EventAgenda
	{
		public EventAgenda()
		{
			Slots = new List<TimeSlot>();
		}

		public String EventName { get; set; }

		public IList<TimeSlot> Slots { get; set; }

		public IList<Talk> Talks
		{
			// Temp before binding slots to a page each
			get
			{
				return Slots.SelectMany (t => t.Talks).ToList();
			}
		}
	}
}