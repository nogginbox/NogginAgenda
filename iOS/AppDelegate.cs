using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using System.IO;

namespace NogginAgenda.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init ();

			/*var pathBefore = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var cacheBefore = Path.Combine (pathBefore, "..", "Library", "Caches");
			var cacheBeforeBits = cacheBefore.Split('/');

			var cacheNow = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User);
			var cacheNowBits = cacheNow[0].Path.Split('/');*/

			String cachePath;
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0))
			{
				var url = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User) [0];
				cachePath = url.Path;
			}
			else
			{
				var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				cachePath = Path.GetFullPath (Path.Combine (documents, "..", "Library", "Caches"));
			}

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			window.RootViewController = App.GetMainPage (cachePath).CreateViewController ();
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

