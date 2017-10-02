
namespace Kit.Utility.OpenXml
{
	public class ParseSettings
	{

		static ParseSettings _defaultSettings = new ParseSettings
		{
		};
		public static ParseSettings DefaultSettings { get { return _defaultSettings; } }

		public ParseProvider[] CustomProviders { set; get; }

	}
}