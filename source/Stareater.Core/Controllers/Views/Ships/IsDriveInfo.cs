using System;
using System.Collections.Generic;
using Stareater.AppData;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Views.Ships
{
	public class IsDriveInfo
	{
		private const string LangContext = "IsDrives";
		
		internal IsDriveType IsDriveType { get; private set; }
		internal int Level { get; private set; }
		
		private readonly IDictionary<string, double> levelVar;
		
		internal IsDriveInfo(IsDriveType isDriveType, int level)
		{
			this.IsDriveType = isDriveType;
			this.Level = level;
			
			this.levelVar = new Var(AComponentType.LevelKey, level).Get;
		}
		
		public string Name 
		{ 
			get
			{
				return Settings.Get.Language[LangContext][this.IsDriveType.NameCode].Text();
			}
		}
		
		public string ImagePath
		{
			get
			{
				return this.IsDriveType.ImagePath;
			}
		}
		
		public double Speed
		{
			get
			{
				return this.IsDriveType.Speed.Evaluate(levelVar);
			}
		}
	}
}
