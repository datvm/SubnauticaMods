using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterGrowth.Patches
{

    [HarmonyPatch(typeof(GrowingPlant))]
    public static class GrowingPlantPatches
    {
        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(GetGrowthDuration)), HarmonyPostfix]
        public static void GetGrowthDuration(ref float __result)
        {
            __result *= C.DurationMultiplier;
        }

    }

}
