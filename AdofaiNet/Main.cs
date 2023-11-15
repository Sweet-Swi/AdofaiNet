using AdofaiNet.Util;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace AdofaiNet
{
	public class Main
	{
		public static Harmony harmony;
		public static bool Enabled = false;
		public static int GameVersion = 0;
		public static string ModPath = "";
		public static UnityModManager.ModEntry Entry;

		public static void Load(UnityModManager.ModEntry modEntry)
		{
			ADOStartup.ModWasAdded(modEntry.Info.Id);
			GameVersion = ReflectionTools.Get<int>(typeof(GCNS), nameof(GCNS.releaseNumber));
			modEntry.Logger.Log($"Game Version: {GameVersion}");
			ModPath = modEntry.Path;
			harmony = new Harmony(modEntry.Info.Id);
			Entry = modEntry;
			modEntry.OnToggle = (entry, value) =>
			{
				Enabled = value;

				if (value)
				{
					harmony.PatchAll(Assembly.GetExecutingAssembly());
				}
				else
				{
					harmony.UnpatchAll(entry.Info.Id);
				}

				return true;
			};
		}
	}
}
