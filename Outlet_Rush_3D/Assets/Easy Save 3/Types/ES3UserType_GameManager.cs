using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Cash_Player", "Stores", "Shoes_Tut", "Wait_Tut", "Billing_Tut", "Task_Count")]
	public class ES3UserType_GameManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameManager() : base(typeof(GameManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GameManager)obj;
			
			writer.WriteProperty("Cash_Player", instance.Cash_Player, ES3Type_int.Instance);
			writer.WriteProperty("Stores", instance.Stores, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(GameManager.Store[])));
			writer.WriteProperty("Shoes_Tut", instance.Shoes_Tut, ES3Type_bool.Instance);
			writer.WriteProperty("Wait_Tut", instance.Wait_Tut, ES3Type_bool.Instance);
			writer.WriteProperty("Billing_Tut", instance.Billing_Tut, ES3Type_bool.Instance);
			writer.WriteProperty("Task_Count", instance.Task_Count, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GameManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Cash_Player":
						instance.Cash_Player = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "Stores":
						instance.Stores = reader.Read<GameManager.Store[]>();
						break;
					case "Shoes_Tut":
						instance.Shoes_Tut = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Wait_Tut":
						instance.Wait_Tut = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Billing_Tut":
						instance.Billing_Tut = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Task_Count":
						instance.Task_Count = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_GameManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameManagerArray() : base(typeof(GameManager[]), ES3UserType_GameManager.Instance)
		{
			Instance = this;
		}
	}
}