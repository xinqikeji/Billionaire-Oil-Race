using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("playerName")]
	public class ES3UserType_PlayerName : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerName() : base(typeof(BlueStellar.Cor.PlayerName)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BlueStellar.Cor.PlayerName)obj;
			
			writer.WritePrivateField("playerName", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BlueStellar.Cor.PlayerName)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "playerName":
					instance = (BlueStellar.Cor.PlayerName)reader.SetPrivateField("playerName", reader.Read<System.String>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PlayerNameArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerNameArray() : base(typeof(BlueStellar.Cor.PlayerName[]), ES3UserType_PlayerName.Instance)
		{
			Instance = this;
		}
	}
}