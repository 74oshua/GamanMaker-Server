using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection.Emit;

namespace GamanMaker
{
	public class Patches
	{
		public class Game_Patch
		{
			[HarmonyPatch(typeof(Game), "Start")]
			[HarmonyPrefix]
			public static void Start_Prefix()
			{
				ZRoutedRpc.instance.Register("RequestSetWeather", new Action<long, ZPackage>(WeatherSystem.RPC_RequestSetWeather));
				ZRoutedRpc.instance.Register("EventSetWeather", new Action<long, ZPackage>(WeatherSystem.RPC_EventSetWeather));
				ZRoutedRpc.instance.Register("RequestSetTime", new Action<long, ZPackage>(WeatherSystem.RPC_RequestSetTime));
				ZRoutedRpc.instance.Register("EventSetTime", new Action<long, ZPackage>(WeatherSystem.RPC_EventSetTime));
				ZRoutedRpc.instance.Register("RequestSync", new Action<long, ZPackage>(WeatherSystem.RPC_RequestSync));
				ZRoutedRpc.instance.Register("EventSync", new Action<long, ZPackage>(WeatherSystem.RPC_EventSync));
				ZRoutedRpc.instance.Register("RequestAdminSync", new Action<long, ZPackage>(WeatherSystem.RPC_RequestAdminSync));
				ZRoutedRpc.instance.Register("EventAdminSync", new Action<long, ZPackage>(WeatherSystem.RPC_EventAdminSync));
				ZRoutedRpc.instance.Register("RequestSetVisible", new Action<long, ZPackage>(WeatherSystem.RPC_RequestSetVisible));
				ZRoutedRpc.instance.Register("EventSetVisible", new Action<long, ZPackage>(WeatherSystem.RPC_EventSetVisible));
			}
		}

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