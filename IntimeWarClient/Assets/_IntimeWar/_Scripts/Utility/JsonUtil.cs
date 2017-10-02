using System;

namespace MechSquad
{
	public static class JsonUtil
	{
		//static JsonSerializerSettings _settings = new JsonSerializerSettings
		//{
		//	TypeNameHandling = TypeNameHandling.Objects,
		//	TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
		//	Formatting = Formatting.Indented,
		//	NullValueHandling = NullValueHandling.Ignore,
		//	DateTimeZoneHandling = DateTimeZoneHandling.Utc
		//};

		//public static string Serialize<T>(T obj, Formatting indent = Formatting.Indented)
		//{
		//	return JsonConvert.SerializeObject(obj, indent, _settings);
		//}
		//public static string Serialize(object obj, Formatting indent = Formatting.Indented)
		//{
		//	return JsonConvert.SerializeObject(obj, indent, _settings);
		//}
		//public static T Deserialize<T>(string str)
		//{
		//	return JsonConvert.DeserializeObject<T>(str, _settings);
		//}
		//public static object Deserialize(string str, Type type)
		//{
		//	return JsonConvert.DeserializeObject(str, type, _settings);
		//}
		
		//public static string SerializeArgs(object[] obj)
		//{
		//	if (obj == null)
		//		return "null";

		//	return JsonConvert.SerializeObject(obj, Formatting.None);
		//}
	}
}
