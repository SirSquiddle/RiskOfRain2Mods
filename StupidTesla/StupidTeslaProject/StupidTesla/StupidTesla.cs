using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace StupidTesla
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.stupidtesla", "StupidTesla", "1.0.0")]

    public class StupidTesla : BaseUnityPlugin
    {

        public float teslaRange = 500000f;

        public void Awake()
        {
            //On.RoR2.Orbs.LightningOrb.Begin += (orig, self) =>
            // {
            //     if(self.lightningType == RoR2.Orbs.LightningOrb.LightningType.Tesla)
            //     {
            //         self.range = teslaRange;
            //     }
            //     orig(self);
            // };

            IL.RoR2.CharacterBody.UpdateTeslaCoil += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdcR4(35)
                );
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, teslaRange);
            };
        }
        
    }
}