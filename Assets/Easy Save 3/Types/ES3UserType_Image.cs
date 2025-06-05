using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute()]
	public class ES3UserType_Image : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Image() : base(typeof(UnityEngine.UI.Image)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.UI.Image)obj;
			
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.UI.Image)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ImageArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ImageArray() : base(typeof(UnityEngine.UI.Image[]), ES3UserType_Image.Instance)
		{
			Instance = this;
		}
	}
}