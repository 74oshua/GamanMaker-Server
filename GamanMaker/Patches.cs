using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace GamanMaker
{
	public class Patches
	{
		public class EnemyHud_Patch
		{
			[HarmonyPatch(typeof(EnemyHud), "ShowHud")]
			[HarmonyPrefix]
			public static bool ShowHud_Prefix()
			{
				return false;
			}
		}
	}
}