using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Workers_count", "Speed_Upgrades_Count", "Capacity_Upgrades_Count")]
	public class ES3UserType_Store_HR_UpgradeManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Store_HR_UpgradeManager() : base(typeof(Store_HR_UpgradeManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Store_HR_UpgradeManager)obj;
			
			writer.WriteProperty("Workers_count", instance.Workers_count, ES3Type_int.Instance);
			writer.WriteProperty("Speed_Upgrades_Count", instance.Speed_Upgrades_Count, ES3Type_int.Instance);
			writer.WriteProperty("Capacity_Upgrades_Count", instance.Capacity_Upgrades_Count, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Store_HR_UpgradeManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Workers_count":
						instance.Workers_count = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Speed_Upgrades_Count":
						instance.Speed_Upgrades_Count = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Capacity_Upgrades_Count":
						instance.Capacity_Upgrades_Count = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Store_HR_UpgradeManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Store_HR_UpgradeManagerArray() : base(typeof(Store_HR_UpgradeManager[]), ES3UserType_Store_HR_UpgradeManager.Instance)
		{
			Instance = this;
		}
	}
}