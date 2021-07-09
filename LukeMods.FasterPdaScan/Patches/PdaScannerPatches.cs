using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPdaScan.Patches
{

    [HarmonyPatch(typeof(PDAScanner))]
    public static class PdaScannerPatches
    {
        static readonly Config C = Config.Instance;

        static bool updated = false;
        static readonly FieldInfo MappingField = typeof(PDAScanner).GetField("mapping", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly FieldInfo CompleteField = typeof(PDAScanner).GetField("complete", BindingFlags.NonPublic | BindingFlags.Static);

        [HarmonyPatch(nameof(Scan)), HarmonyPrefix]
        public static void Scan()
        {
            var multiplier = C.ScanTimeMultiplier;

            // For scanned fragments
            var techType = PDAScanner.scanTarget.techType;
            var scanned = (CompleteField.GetValue(null) as HashSet<TechType>)?.Contains(techType) == true;

            if (scanned && PDAScanner.scanTarget.progress == 0)
            {
                var gameObject = PDAScanner.scanTarget.gameObject;
                if (gameObject != null)
                {
                    gameObject.SendMessage("OnScanBegin", UnityEngine.SendMessageOptions.DontRequireReceiver);
                }

                PDAScanner.scanTarget.progress = 1 - multiplier;
                return;
            }

            // For non-scanned: try to update all
            if (updated)
            {
                return;
            }

            var mapping = MappingField.GetValue(null) as Dictionary<TechType, PDAScanner.EntryData>;

            if (mapping == null)
            {
                return;
            }

            foreach (var item in mapping)
            {
                item.Value.scanTime *= multiplier;
            }

            updated = true;
        }

    }
}
