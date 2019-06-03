using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace BetterMedkit
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.bettermedkit", "BetterMedkit", "1.0.1")]

    public class BetterMedkit : BaseUnityPlugin
    {
        public float cdRedPerStack = 0.95f;
        public float healDelay = 0.55f;
        public float delayCap = 0.55f;
        public void Awake()
        {
            On.RoR2.CharacterBody.AddTimedBuff += (orig, self, buffType, duration) =>
            {
                if (self.inventory && buffType == BuffIndex.MedkitHeal)
                {
                    int itemCount = self.inventory.GetItemCount(ItemIndex.Medkit);
                    duration = delayCap + healDelay * Mathf.Pow(cdRedPerStack, itemCount - 1);
                }

                orig(self, buffType, duration);
            };
        }
    }
}