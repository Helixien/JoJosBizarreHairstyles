using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Jojo
{
    [DefOf]
    public static class JojoDefOf
    {
        public static GeneDef JoJo_Hair_BizarreOnly;
    }

    [HarmonyPatch(typeof(GeneDef), "Icon", MethodType.Getter)]
    public static class GeneDef_Icon_Patch
    {
        public static List<Texture2D> allIcons;
        public static int curWindowId;
        public static Texture2D prevTexture;
        public static bool Prefix(GeneDef __instance, ref Texture2D __result)
        {
            if (__instance == JojoDefOf.JoJo_Hair_BizarreOnly)
            {
                var windowID = Find.WindowStack.currentlyDrawnWindow?.GetHashCode() ?? 666;
                if (allIcons is null)
                {
                    allIcons = JojoMod.modPack.textures.contentList.Where(x => x.Key.Contains(JojoDefOf.JoJo_Hair_BizarreOnly.iconPath)).Select(x => x.Value).ToList();
                }
                Rand.PushState();
                Rand.Seed = windowID;
                if (curWindowId == windowID)
                {
                    __result = prevTexture;
                }
                else
                {
                    __result = allIcons.Where(x => x != prevTexture).RandomElement();
                    curWindowId = windowID;
                    prevTexture = __result;
                }
                Rand.PopState();
                return false;
            }
            return true;
        }
    }
    public class JojoMod : Mod
    {
        public static ModContentPack modPack;
        public JojoMod(ModContentPack pack) : base(pack)
        {
			new Harmony("JojoMod").PatchAll();
            modPack = pack;
        }
    }
}
