using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NogginAgenda.Data;
using Xamarin.Forms;
using NogginAgenda.DataExchange.Model;
using PCLStorage;

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
			var eventData = new EventAgenda {
				EventName = "DDD North 2014"
			};

			foreach (var t in talksJson)
			{
				var newTalk = new Talk
				{
					Title = t.title,
					Description = t.description,
					Speaker = new Speaker
					{
						Name = t.speaker,
						PictureUrl = t.speakerimage,
						WebsiteUrl = t.speakerlink
					}
				};

				var newSlot = new TimeSlot(t.slot);
				var existingSlot = eventData.Slots.FirstOrDefault (s => newSlot.Equals(s));

				if (existingSlot == null) {
					eventData.Slots.Add(newSlot);
				}

				(existingSlot ?? newSlot).Talks.Add (newTalk);
			}
				
			// Link up timeslots
			foreach (var slot in eventData.Slots)
			{
				foreach (var talk in slot.Talks)
				{
					talk.TimeSlot = slot;
				}
			}

			eventData.Slots = eventData.Slots.OrderBy (s => s.StartTime).ToList();

			return eventData;
		}

		private static String GetJson()
		{
			DateTime saveFileAge;

			String json;

			var rootFolder = FileSystem.Current.LocalStorage;
			var folder = rootFolder.CreateFolderAsync("Caches", CreationCollisionOption.OpenIfExists).Result;

			try{
				var file = folder.GetFileAsync("agenda.json").Result;
				json = file.ReadAllTextAsync().Result;

				// Todo: Get saveFileAge from the file
				saveFileAge = DateTime.Now.AddMinutes(-5);
			}
			catch(Exception) {
				saveFileAge = DateTime.MinValue;
			}

			using (var client = new HttpClient ())
			{
				client.DefaultRequestHeaders.IfModifiedSince = saveFileAge;

				var response = client.GetAsync(RemoteDataPath).Result;

				if (response.IsSuccessStatusCode)
				{
					json = response.Content.ReadAsStringAsync ().Result;
				}
				else
				{
					switch (response.StatusCode)
					{
						case HttpStatusCode.NotModified:
							json = "";
							break;
						case HttpStatusCode.NotFound:
							// Someone has moved the data, ask user to check for new version
							json = "";
							break;
					default:
						throw new Exception ("Network issue, I should show you summat nice");
					}

				}


			}

			return json;
		}
	}
}