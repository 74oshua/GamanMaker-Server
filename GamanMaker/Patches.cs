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
		public class EnemyHud_Patch
		{
			[HarmonyPatch(typeof(EnemyHud), "ShowHud")]
			[HarmonyPrefix]
			public static bool ShowHud_Prefix()
			{
				return false;
			}
		}

		public class Console_Patch
		{
			public static void InputText_Patch(String[] array)
			{
				UnityEngine.Debug.Log("x");
			}

			[HarmonyPatch(typeof(Console), "InputText")]
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> InputText_Transplier(IEnumerable<CodeInstruction> instructions)
			{
				bool matches = false;
				foreach (CodeInstruction ins in instructions)
				{
					yield return ins;
					if (ins.opcode == OpCodes.Stelem_I2)
					{
						matches = true;
					}
					else
					{
						matches = false;
					}

					if (ins.Calls(typeof(String).GetMethod("Split")) && matches)
					{
						yield return new CodeInstruction(OpCodes.Call, typeof(Console_Patch).GetMethod("InputText_Patch"));
					}
				}
			}
		}
	}
}