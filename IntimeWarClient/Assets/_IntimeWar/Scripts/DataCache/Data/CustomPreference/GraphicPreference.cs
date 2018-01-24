using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace IntimeWar
{
	public class GraphicPref
	{
        public enum ParticleEffectLevelEnum
        {
            Low = 1,
            Middle = 2,
            High = 3,
        }
        public ParticleEffectLevelEnum ParticleEffectLevel = ParticleEffectLevelEnum.High;

        public enum VisualEffectLevelEnum
        {
            Low = 1,
            Middle = 2,
            High = 3,
        }
        public VisualEffectLevelEnum VisualEffectLevel = VisualEffectLevelEnum.High;

        public enum FragmentCountLevelEnum
		{
			Poor = 1,
			Few = 2,
			More = 3,
			Many = 4,
		}
		public FragmentCountLevelEnum FragmentCountLevel = FragmentCountLevelEnum.Many;

		public enum FragmentExistDurationEnum
		{
			Short = 1,
			Medium = 3,
			Long = 7,
		}
		public FragmentExistDurationEnum FragmentExistDurationLevel = FragmentExistDurationEnum.Long;
		public float FragmentExistDuration { get { return (int)FragmentExistDurationLevel * 5; } }
		
		public bool FragmentFragable = true;
        public bool FullScreen = false;
        public bool Antialiasing = false;
        public bool Shadow = false;
        public bool CameraEffect = false;
        public bool DynamicScene = false;
        public bool ShowBattleValue = false;
    }
}
