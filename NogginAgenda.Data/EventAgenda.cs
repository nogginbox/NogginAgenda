using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NogginAgenda.Data
{
	public class EventAgenda
	{
		public EventAgenda()
		{
			Slots = new ObservableCollection<TimeSlot>();
		}

		public String EventName { get; set; }

		public ObservableCollection<TimeSlot> Slots { get; set; }

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