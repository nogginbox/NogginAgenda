using System;
using Xamarin.Forms;
using NogginAgenda.Data;
using System.Collections.Generic;

namespace NogginAgenda
{
	public class App
	{
		public static EventAgenda EventData { get; private set; }

		public static Page GetMainPage ()
		{	
			InitEventData ();

			var page = new EventsListPage();
			page.BindingContext = EventData;

			return page;
		}

		private static void InitEventData()
		{
			// Todo: Check if newer event data available online
			// If newer: Download and cache
			// If older: Load cached version

			EventData = new EventAgenda
			{
				EventName = "DDD North 2014",
				Slots = new List<TimeSlot>
				{
					new TimeSlot {
						StartTime = new DateTime(2014, 01,01, 9,0,0),
						EndTime = new DateTime(2014, 01,01, 10,0,0),
						Talks = new List<Talk>
						{
							new Talk {
								Title = "Talk One"
							},
							new Talk {
								Title = "Talk Two"
							}
						}
					},
					new TimeSlot {
						StartTime = new DateTime(2014, 01,01, 9,0,0),
						EndTime = new DateTime(2014, 01,01, 10,0,0),
						Talks = new List<Talk>
						{
							new Talk {
								Title = "Talk Aaaaa"
							}
						}
					},
				}
			};
		}


	}
}

