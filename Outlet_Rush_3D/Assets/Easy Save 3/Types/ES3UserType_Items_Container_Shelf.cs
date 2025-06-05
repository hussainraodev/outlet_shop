using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Items_Count")]
	public class ES3UserType_Items_Container_Shelf : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Items_Container_Shelf() : base(typeof(Items_Container_Shelf)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Items_Container_Shelf)obj;
			
			writer.WriteProperty("Items_Count", instance.Items_Count, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Items_Container_Shelf)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Items_Count":
						instance.Items_Count = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Items_Container_ShelfArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Items_Container_ShelfArray() : base(typeof(Items_Container_Shelf[]), ES3UserType_Items_Container_Shelf.Instance)
		{
			Instance = this;
		}
	}
}