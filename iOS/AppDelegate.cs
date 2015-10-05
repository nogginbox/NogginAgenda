using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using Xamarin.Forms;
using System.IO;
using NogginAgenda;

namespace NogginAgenda.iOS
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
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

            global::Xamarin.Forms.Forms.Init ();

            LoadApplication (new App (cachePath));

            return base.FinishedLaunching (app, options);
        }
    }
}