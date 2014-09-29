using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NogginAgenda.Data;
using Xamarin.Forms;
using NogginAgenda.DataExchange.Model;
using PCLStorage;
using System.Threading.Tasks;

namespace NogginAgenda
{
	public class App
	{
		private const String RemoteDataPath = "http://www.garsonix.co.uk/temp/dddnorth2014.js";
		private const String JsonCacheFileName = "agenda.json";
		private static String _cacheFolder;


		public static EventAgenda EventData { get; private set; }

		public static Page GetMainPage (String cacheFolder)
		{
			_cacheFolder = cacheFolder;
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

			String json = null;

			var folder = GetCacheFolder();

			try
			{
				if(folder.CheckExistsAsync(JsonCacheFileName).Result == ExistenceCheckResult.NotFound)
				{
					throw new Exception("No file yet");
				}

				var file = folder.GetFileAsync(JsonCacheFileName).Result;
				var thing = file.Path.Split('/');

				json = ReadFile(file).Result;
				// Todo: throw exception if json is bad

				saveFileAge = GetAgeFromJson(json);
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
					json = response.Content.ReadAsStringAsync().Result;
					SaveJsonFile (json);
				}
				else
				{
					switch (response.StatusCode)
					{
						case HttpStatusCode.NotModified:
							// Cached is newer. So just use this
							break;
						case HttpStatusCode.NotFound:
							// Someone has moved the data, ask user to check for new version
							if(String.IsNullOrWhiteSpace(json))
							{
								throw new Exception("The data for this conference has moved. You may need to update this app.");	
							}
							// Show a warning saying the data has moved so this may not be up to date
							// ?? page.DisplayAlert ??
							break;
						default:
							if (String.IsNullOrWhiteSpace (json)) {
							throw new Exception ("Network issue, I should show you summat nice");
							}
							// Todo: Show warning?: Data may be old
							break;
					}
				}
			}

			return json;
		}

		private static IFolder GetCacheFolder()
		{
			if (_cacheFolder != null) {

				var cacheFolder = FileSystem.Current.GetFolderFromPathAsync (_cacheFolder).Result;
				return cacheFolder;
			}

			var rootFolder = FileSystem.Current.LocalStorage;
			return rootFolder.CreateFolderAsync("Caches/AgendaApp", CreationCollisionOption.OpenIfExists).Result;
		}

		private static void SaveJsonFile(String json)
		{
			json = SetAgeOnJson(json, DateTime.Now);
			var folder = GetCacheFolder();
			var file = folder.CreateFileAsync (JsonCacheFileName, CreationCollisionOption.ReplaceExisting).Result;
			file.WriteAllTextAsync(json).Wait();
		}

		private static DateTime GetAgeFromJson(String json)
		{
			var dateRegExp = new Regex ("\"createdDate\": \"(?<createdDate>.*?)\"");
			var matches = dateRegExp.Match (json);
			var createdDateMatch = matches.Groups ["createdDate"].Value;
			var returnDate = DateTime.MinValue;
			DateTime.TryParse (createdDateMatch, out returnDate);

			return returnDate;
		}

		private static async Task<string> ReadFile(IFile f){
			return await Task.Run(() => f.ReadAllTextAsync ()).ConfigureAwait(false);
		}

		private static String SetAgeOnJson(String json, DateTime age)
		{
			var regex = new Regex("(.*\"createdDate\":) \"([^\"]+)(\".*)");
			var replace = String.Format("$1 \"{0:yyyy-MM-dd H:mm}$3", age);
			var replaced = regex.Replace(json, replace);
			return replaced;
		}
	}
}