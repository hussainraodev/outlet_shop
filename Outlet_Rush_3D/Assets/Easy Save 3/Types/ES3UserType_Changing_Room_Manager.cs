using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("ChangingRoomsUnlocked")]
	public class ES3UserType_Changing_Room_Manager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Changing_Room_Manager() : base(typeof(Changing_Room_Manager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Changing_Room_Manager)obj;
			
			writer.WriteProperty("ChangingRoomsUnlocked", instance.ChangingRoomsUnlocked, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Changing_Room_Manager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "ChangingRoomsUnlocked":
						instance.ChangingRoomsUnlocked = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Changing_Room_ManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Changing_Room_ManagerArray() : base(typeof(Changing_Room_Manager[]), ES3UserType_Changing_Room_Manager.Instance)
		{
			Instance = this;
		}
	}
}