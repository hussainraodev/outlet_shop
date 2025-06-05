using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("saveActive", "saveChildren", "enabled")]
	public class ES3UserType_ES3AutoSave : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ES3AutoSave() : base(typeof(ES3AutoSave)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (ES3AutoSave)obj;
			
			writer.WriteProperty("saveActive", instance.saveActive, ES3Type_bool.Instance);
			writer.WriteProperty("saveChildren", instance.saveChildren, ES3Type_bool.Instance);
			writer.WriteProperty("enabled", instance.enabled, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (ES3AutoSave)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "saveActive":
						instance.saveActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "saveChildren":
						instance.saveChildren = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enabled":
						instance.enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ES3AutoSaveArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ES3AutoSaveArray() : base(typeof(ES3AutoSave[]), ES3UserType_ES3AutoSave.Instance)
		{
			Instance = this;
		}
	}
}