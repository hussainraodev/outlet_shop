using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Cash_Handler", "HasCashierAI", "Cashier")]
	public class ES3UserType_Customer_Billing_Manager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Customer_Billing_Manager() : base(typeof(Customer_Billing_Manager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Customer_Billing_Manager)obj;
			
			writer.WritePropertyByRef("Cash_Handler", instance.Cash_Handler);
			writer.WriteProperty("HasCashierAI", instance.HasCashierAI, ES3Type_bool.Instance);
			writer.WritePropertyByRef("Cashier", instance.Cashier);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Customer_Billing_Manager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Cash_Handler":
						instance.Cash_Handler = reader.Read<Cash_Counter>();
						break;
					case "HasCashierAI":
						instance.HasCashierAI = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Cashier":
						instance.Cashier = reader.Read<UnityEngine.GameObject>(ES3Type_GameObject.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Customer_Billing_ManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Customer_Billing_ManagerArray() : base(typeof(Customer_Billing_Manager[]), ES3UserType_Customer_Billing_Manager.Instance)
		{
			Instance = this;
		}
	}
}