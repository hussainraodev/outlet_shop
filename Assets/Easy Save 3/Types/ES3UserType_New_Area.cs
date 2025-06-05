using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("CashCollected", "IsUnlocked")]
	public class ES3UserType_New_Area : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_New_Area() : base(typeof(New_Area)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (New_Area)obj;
			
			writer.WriteProperty("CashCollected", instance.CashCollected, ES3Type_int.Instance);
			writer.WriteProperty("IsUnlocked", instance.IsUnlocked, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (New_Area)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CashCollected":
						instance.CashCollected = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "IsUnlocked":
						instance.IsUnlocked = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_New_AreaArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_New_AreaArray() : base(typeof(New_Area[]), ES3UserType_New_Area.Instance)
		{
			Instance = this;
		}
	}
}