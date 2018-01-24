

namespace MechSquad
{
	[Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.Fields)]
	public class CustomPref
	{
        static GraphicPref _graphic = new GraphicPref();
		public static GraphicPref Graphic
        {
            get
            {
                if (_graphic == null)
                    _graphic = new GraphicPref();
                return _graphic;
            }
            set
            {
                _graphic = value;
            }
        }
		
        static ControlPref _control = new ControlPref();
		public static ControlPref Control
        {
            get
            {
                if (_control == null)
                    _control = new ControlPref();
                return _control;
            }
            set
            {
                _control = value;
            }
        }
		
        static KeysPref _keys = new KeysPref();
        public static KeysPref Keys
        {
            get
            {
                if (_keys == null)
                    _keys = new KeysPref();
                return _keys;
            }
            set
            {
                _keys = value;
            }
        }
    }
}
