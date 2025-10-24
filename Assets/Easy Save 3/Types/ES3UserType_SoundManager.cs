using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isOffSound")]
	public class ES3UserType_SoundManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SoundManager() : base(typeof(BlueStellar.Cor.SoundManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BlueStellar.Cor.SoundManager)obj;
			
			writer.WritePrivateField("isOffSound", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BlueStellar.Cor.SoundManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isOffSound":
					instance = (BlueStellar.Cor.SoundManager)reader.SetPrivateField("isOffSound", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SoundManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SoundManagerArray() : base(typeof(BlueStellar.Cor.SoundManager[]), ES3UserType_SoundManager.Instance)
		{
			Instance = this;
		}
	}
}