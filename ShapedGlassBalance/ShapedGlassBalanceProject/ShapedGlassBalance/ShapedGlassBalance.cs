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

    [BepInPlugin("com.Squiddle.shapedglassbalance", "ShapedGlassBalance", "1.0.3")]

    public class ShapedGlassBalance : BaseUnityPlugin
    {
        public void Awake()
        {
            object instr = null;
            object instr2 = null;
            object instr3 = null;

            IL.RoR2.CharacterBody.CalcLunarDaggerPower += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdcR4(2),
                    x => x.MatchLdarg(0)
                    );
                c.Index += 2;
                instr = c.Next.Operand;
                c.Index += 1;
                instr2 = c.Next.Operand;
                c.Index += 1;
                instr3 = c.Next.Operand;
            };

            IL.RoR2.CharacterBody.RecalculateStats += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdloc(40), 
                    x => x.MatchLdloc(23), 
                    x => x.MatchLdcR4(1),
                    x => x.MatchSub(),
                    x => x.MatchAdd()
                    );
                c.Index += 2;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float, CharacterBody, float>>((num23, cbody) =>
                {
                    if (cbody.inventory)
                    {
                        num23 = 1 + cbody.inventory.GetItemCount(ItemIndex.LunarDagger);
                        return num23;
                    }
                    else
                    {
                        return 1;
                    }
                });
                /*c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Call, instr);
                c.Emit(OpCodes.Ldc_I4_S, instr2);
                c.Emit(OpCodes.Callvirt, instr3);
                //c.Emit(OpCodes.Callvirt, typeof(Inventory).GetMethod("GetItemCount", new System.Type[] { typeof(ItemIndex) }));
                c.Emit(OpCodes.Conv_R4);
                c.Emit(OpCodes.Ldc_I4_1);
                c.Emit(OpCodes.Conv_R4);
                c.Emit(OpCodes.Add);
                c.Emit(OpCodes.Stloc, 23);*/
            };

            //On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            //{
            //    Debug.Log("Start health : " + self.health);
            //    Debug.Log("Start shield : " + self.shield);
            //    Debug.Log("Input damage : " + damageInfo.damage);
            //    orig(self, damageInfo);
            //    Debug.Log("End health : " + self.health);
            //    Debug.Log("End shield : " + self.shield);
            //    Debug.Log("Output damage : " + damageInfo.damage);
            //    Debug.Log("" );

            //};
        }
    }
}