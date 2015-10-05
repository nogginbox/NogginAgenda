using System;
using System.Collections.Generic;

namespace NogginAgenda.DataExchange.Model
{
	public class DataHolder
	{
        public String EventName { get; set; }
		public List<TalkData> Data { get; set;}
	}
}