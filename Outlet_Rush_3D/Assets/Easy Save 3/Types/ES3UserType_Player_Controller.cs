using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Holding_Capacity", "CanPick")]
	public class ES3UserType_Player_Controller : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Player_Controller() : base(typeof(Player_Controller)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Player_Controller)obj;
			
			writer.WriteProperty("Holding_Capacity", instance.Holding_Capacity, ES3Type_int.Instance);
			writer.WriteProperty("CanPick", instance.CanPick, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Player_Controller)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Holding_Capacity":
						instance.Holding_Capacity = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "CanPick":
						instance.CanPick = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Player_ControllerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Player_ControllerArray() : base(typeof(Player_Controller[]), ES3UserType_Player_Controller.Instance)
		{
			Instance = this;
		}
	}
}