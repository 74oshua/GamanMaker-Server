using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace GamanMaker
{
	[BepInPlugin("org.bepinex.plugins.gamanmaker", "GamanMaker-Server", "0.1.0.0")]
	public class GamanMaker : BaseUnityPlugin
	{
		public static List<string> invisible_players = new List<string>();

		public void Awake()
		{
			System.Console.WriteLine("Starting GamanMaker-Server");
			Harmony.CreateAndPatchAll(typeof(Patches.Game_Patch));
			Harmony.CreateAndPatchAll(typeof(Patches.EnemyHud_Patch));
			// Harmony.CreateAndPatchAll(typeof(Patches.Console_Patch));
		}
	}
}
