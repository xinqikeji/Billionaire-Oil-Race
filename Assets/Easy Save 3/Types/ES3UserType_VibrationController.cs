using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("isOffVibration")]
	public class ES3UserType_VibrationController : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_VibrationController() : base(typeof(BlueStellar.Cor.VibrationController)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (BlueStellar.Cor.VibrationController)obj;
			
			writer.WritePrivateField("isOffVibration", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (BlueStellar.Cor.VibrationController)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "isOffVibration":
					instance = (BlueStellar.Cor.VibrationController)reader.SetPrivateField("isOffVibration", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_VibrationControllerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_VibrationControllerArray() : base(typeof(BlueStellar.Cor.VibrationController[]), ES3UserType_VibrationController.Instance)
		{
			Instance = this;
		}
	}
}