using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using NogginAgenda.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NogginAgenda
{
	public class App
	{
		private const String RemoteDataPath = "http://www.garsonix.co.uk/temp/dddnorth2014.js";

		public static EventAgenda EventData { get; private set; }

		public static Page GetMainPage ()
		{	
			InitEventData ();

			var slotsPage = new CarouselPage {
				Title = "DDD North 2014",
				ItemsSource = EventData.Slots,
				ItemTemplate =  new DataTemplate(() =>
				{
					return new TalksListPage();
				})
			};

			return new NavigationPage(slotsPage);
		}

		private static void InitEventData()
		{
			// Todo: Check if newer event data available online
			// If newer: Download and cache
			// If older: Load cached version

			var json = GetJson();

			// Now parse with JSON.Net
			var jsonData = JsonConvert.DeserializeObject<DataHolder>(json);

			EventData = CreateEventDataFromJsonData(jsonData.Data);
		}

		private static EventAgenda CreateEventDataFromJsonData(IList<TalkData> talksJson)
		{
			var talks = talksJson.Select (t => new Talk { Title = t.title });

			var eventData = new EventAgenda
			{
				EventName = "DDD North 2014",
				Slots = new List<TimeSlot>
				{
					new TimeSlot {
						StartTime = new DateTime(2014, 01,01, 9,0,0),
						EndTime = new DateTime(2014, 01,01, 10,0,0),
						Talks = talks.Take(10).ToList()
					},
					new TimeSlot {
						StartTime = new DateTime(2014, 01,01, 9,0,0),
						EndTime = new DateTime(2014, 01,01, 10,0,0),
						Talks = talks.Skip(10).Take(10).ToList()
					},
				}
			};

			return eventData;
		}

		private static String GetJson()
		{
			String json;
			using (var webClient = new HttpClient ())
			{
				json = webClient.GetStringAsync (RemoteDataPath).Result;
			}
			// Todo: catch errors, 404 prob means newer version

			return json;
		}
	}
}

