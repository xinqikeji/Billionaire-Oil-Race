using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("indexLevel")]
	public class ES3UserType_ArenaSpawner : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ArenaSpawner() : base(typeof(BlueStellar.Cor.ArenaSpawner)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BlueStellar.Cor.ArenaSpawner)obj;
			
			writer.WritePrivateField("indexLevel", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BlueStellar.Cor.ArenaSpawner)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "indexLevel":
					instance = (BlueStellar.Cor.ArenaSpawner)reader.SetPrivateField("indexLevel", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ArenaSpawnerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ArenaSpawnerArray() : base(typeof(BlueStellar.Cor.ArenaSpawner[]), ES3UserType_ArenaSpawner.Instance)
		{
			Instance = this;
		}
	}
}