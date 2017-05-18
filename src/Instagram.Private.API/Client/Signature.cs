namespace Instagram.Private.API.Client
{
	internal class Signature
	{
		public static Signature Latest => new Signature();

		public string PrivateKey => "299a77ffe98a252a20e1fb6bc87df721b90fe70c4cb327391b2dacaffd187f99";
		public string Version => "4";
		public string ApplicationVesion => "10.21.0";
	}
}
