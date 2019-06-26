using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using System;

namespace ShapedGlassBalance
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.shapedglassbalance", "ShapedGlassBalance", "1.0.4")]

    public class ShapedGlassBalance : BaseUnityPlugin
    {
        public void Awake()
        {
            IL.RoR2.CharacterBody.RecalculateStats += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdloc(42),
                    x => x.MatchLdcR4(2),
                    x => x.MatchLdloc(24), 
                    x => x.MatchConvR4()
                    );
                c.Index += 1;
                c.Remove();
                c.Index += 2;
                c.RemoveRange(3);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, CharacterBody, float>>((num24, cbody) =>
                {
                    if (cbody.inventory)
                    {
                        num24 = cbody.inventory.GetItemCount(ItemIndex.LunarDagger);
                        return num24;
                    }
                    else
                    {
                        return 1f;
                    }
                });
                //Debug.Log(il);
            };
            
        }
    }
}