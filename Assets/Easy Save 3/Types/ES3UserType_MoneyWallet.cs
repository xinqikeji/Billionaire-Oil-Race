using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("ammountMoney")]
	public class ES3UserType_MoneyWallet : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MoneyWallet() : base(typeof(BlueStellar.Cor.MoneyWallet)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BlueStellar.Cor.MoneyWallet)obj;
			
			writer.WritePrivateField("ammountMoney", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BlueStellar.Cor.MoneyWallet)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "ammountMoney":
					instance = (BlueStellar.Cor.MoneyWallet)reader.SetPrivateField("ammountMoney", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_MoneyWalletArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MoneyWalletArray() : base(typeof(BlueStellar.Cor.MoneyWallet[]), ES3UserType_MoneyWallet.Instance)
		{
			Instance = this;
		}
	}
}