using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
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
		private static String _errorMessage = "";

		public static EventAgenda EventData { get; private set; }

        static App()
        {
            EventData = new EventAgenda ();
        }


		public static Page GetMainPage (String cacheFolder = null)
		{
			_cacheFolder = cacheFolder;
            var slotsPage = new CarouselPage();
            slotsPage.Appearing += OnSlotsPageAppearing;

            return new NavigationPage(
                slotsPage
            );
		}

        protected static void OnSlotsPageAppearing(object sender, EventArgs args)
        {
            // Show loading page as model.
            // Loading page will pop itself once data is loaded
            var page = sender as CarouselPage;
            page.Navigation.PushModalAsync (new LoadingPage(page));
            page.Appearing -= OnSlotsPageAppearing;
        }

		public static async Task InitEventData()
		{
			try{
				var json = await GetJson();

				// Now parse with JSON.Net
				var jsonData = JsonConvert.DeserializeObject<DataHolder>(json);

				UpdateEventDataFromJsonData(EventData, jsonData.Data);
			}
			catch(Exception e) {
				_errorMessage = e.Message;
			}
		}

		private static void UpdateEventDataFromJsonData(EventAgenda eventData, IEnumerable<TalkData> talksJson)
		{
		    eventData.EventName = "DDD North 2014";

		    var slots = new List<TimeSlot>();

			foreach (var t in talksJson)
			{
				var newTalk = new Talk
				{
					Title = t.title,
                    Description = t.description,
                    Room = t.room,
					Speaker = new Speaker
					{
						Name = t.speaker,
						PictureUrl = t.speakerimage,
						WebsiteUrl = t.speakerlink,
                        TwitterUrl = String.IsNullOrEmpty(t.speakertwitter) ? null : String.Format("https://www.twitter.com/{0}",t.speakertwitter)
					}
				};

				var newSlot = new TimeSlot(t.slot);
				var existingSlot = slots.FirstOrDefault (s => newSlot.Equals(s));

				if (existingSlot == null) {
					slots.Add(newSlot);
				}

				(existingSlot ?? newSlot).Talks.Add (newTalk);
			}
				
			// Link up timeslots
			foreach (var slot in slots)
			{
				foreach (var talk in slot.Talks)
				{
					talk.TimeSlot = slot;
				}

                slot.Talks = slot.Talks.OrderBy (t => t.Room).ToList();
			}

            // Add slots to eventData
		    foreach (var slot in slots.OrderBy (s => s.StartTime))
		    {
		        eventData.Slots.Add(slot);
		    }
		}

		private static async Task<String> GetJson()
		{
			DateTime saveFileAge;

			String json = null;

			var folder = GetCacheFolder();

			try
			{
				if(! await FileExists(folder, JsonCacheFileName))
				{
					throw new Exception("No file yet");
				}

				var file = await folder.GetFileAsync(JsonCacheFileName);

				json = ReadFile(file).Result;
				// Todo: throw exception if json is bad

				saveFileAge = GetAgeFromJson(json);
			}
			catch(Exception) {
				saveFileAge = DateTime.MinValue;
			}

			using (var client = new HttpClient ())
			{
			    if (saveFileAge != DateTime.MinValue)
			    {
			        client.DefaultRequestHeaders.IfModifiedSince = saveFileAge;
			    }

			    HttpResponseMessage response;
			    try
			    {
                    response = await client.GetAsync(RemoteDataPath);
			    }
			    catch (Exception e)
			    {
                    if (String.IsNullOrEmpty(json))
                    {
                        throw new Exception("We can't connect to the Internet right now to get the agenda.");
                    }
                    return json;
			    }
				

				if (response.IsSuccessStatusCode)
				{
					json = response.Content.ReadAsStringAsync().Result;
					await SaveJsonFile (json);
				}
				else
				{
					switch (response.StatusCode)
					{
						case HttpStatusCode.NotModified:
							// Cached is newer. So just use this
							return json;
						case HttpStatusCode.NotFound:
							// Someone has moved the data, ask user to check for new version
							_errorMessage = "The data for this conference has moved. Please check to see if there is an updated version of this app.";
							return json;
					default:
						if (String.IsNullOrEmpty (json)) {
							throw new Exception ("We can't connect to the Internet right now to get the agenda.");
						}
						return json;
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
			return rootFolder.CreateFolderAsync("Caches", CreationCollisionOption.OpenIfExists).Result;
		}

		private static async Task SaveJsonFile(String json)
		{
			json = SetAgeOnJson(json, DateTime.Now);
			var folder = GetCacheFolder();
			var file = await folder.CreateFileAsync (JsonCacheFileName, CreationCollisionOption.ReplaceExisting);
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

	    private static async Task<bool> FileExists(IFolder folder, String fileName)
	    {
	        var existStatus = await folder.CheckExistsAsync(fileName);
	        return existStatus != ExistenceCheckResult.NotFound;
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