using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using RusherNetLib.Core;

namespace RusherNetLib
{
	public class Message : IMessage
	{
		public int Sended { get; private set; }
		public int Received { get; private set; }

		private Dictionary<string, object> messages;

		internal Message()
		{
			Sended = 0;
			Received = 0;
			messages = new Dictionary<string, object>();
		}
		internal Message(int sended) : this()
		{
			Sended = sended;
		}
		internal Message(byte[] data) : this()
		{
			Received = data.Length;
			var xml = XElement.Parse(Encoding.UTF8.GetString(data)).Descendants("data");
			messages = xml.ToDictionary(x => x.Attribute("key").Value, x => (object)x.Attribute("value").Value);
		}
		public byte[] GetBytes()
		{
			var xml = new XElement("msg", messages.Select(x => new XElement("data", new XAttribute("key", x.Key), new XAttribute("value", x.Value))));
			return Encoding.UTF8.GetBytes(xml.ToString());
		}
		public dynamic this[string name]
		{
			get
			{
				if (messages.TryGetValue(name, out var outValue))
					return outValue;
				throw new Exception("Обьект не найден");
			}
			set
			{
				if (messages.TryGetValue(name, out var outValue))
				{
					messages[name] = value;
				}
				else
				{
					messages.Add(name, value);
				}
			}
		}
	}
}
