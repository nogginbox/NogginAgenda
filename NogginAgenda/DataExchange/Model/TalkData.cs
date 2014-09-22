using System;
using System.Collections.Generic;

namespace NogginAgenda.DataExchange.Model
{
	public class TalkData
	{
		public String speakerlink {get;set;}
		public String speakerimage { get; set; }
		public String speaker { get; set; }
		public String description { get; set;}
		public String title { get; set; }

		/// <summary>
		/// Format: yyyy-mm-dd h:mm,yyyy-mm-dd h:mm
		/// </summary>
		public String slot { get; set; }
	}
}