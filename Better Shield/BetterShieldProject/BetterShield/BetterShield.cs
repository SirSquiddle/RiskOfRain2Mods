using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace BetterShield
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.bettershield", "BetterShield", "1.0.2")]

    public class BetterShield : BaseUnityPlugin
    {
        public float damageIncrease;
        public float damageReduction;

        public void Awake()
        {
            damageIncrease = 1.3f;
            damageReduction = 0.85f;
            
            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };

            //Makes the dash inflictor be the ToolbotBody gameobject

            IL.EntityStates.Toolbot.ToolbotDash.FixedUpdate += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchStfld<RoR2.DamageInfo>("attacker"),
                    x => x.MatchDup(),
                    x => x.MatchLdarg(0)
                );
                c.Index -= 1;
                var instr = c.Next.Operand;
                c.Index -= 1;
                c.Emit(OpCodes.Dup);
                c.Index += 4;
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Call, instr);
                c.Emit(OpCodes.Stfld, typeof(RoR2.DamageInfo).GetField("inflictor"));
                //Debug.Log(il.ToString());
            };

            //manages ukulele, Mage M2 lightnings and tesla damage increase on shield

            On.RoR2.Orbs.LightningOrb.Begin += (orig, self) =>
            {                
                if((self.lightningType == RoR2.Orbs.LightningOrb.LightningType.Tesla || self.lightningType == RoR2.Orbs.LightningOrb.LightningType.Ukulele || self.lightningType == RoR2.Orbs.LightningOrb.LightningType.BFG))
                {
                    if(self.target.healthComponent.shield > 0)
                    {
                        //Debug.Log("Damage increased by shield + decrease canceled");
                        //Debug.Log("Orb Damage value input : " + self.damageValue);
                        float damageleftaftershieldincrease = self.damageValue * damageIncrease - self.target.healthComponent.shield;

                        if (damageleftaftershieldincrease > 0)
                        {
                            self.damageValue = (self.target.healthComponent.shield + damageleftaftershieldincrease / damageIncrease) / damageReduction; //the /damageReduction cancels the reduction from the TakeDamage hook since I can't identify those types of damage
                        }
                        else
                        {
                            self.damageValue = (self.damageValue * damageIncrease) / damageReduction; //the /damageReduction cancels the reduction from the TakeDamage hook since I can't identify those types of damage
                        }
                    }
                }
                orig(self);
            };

            //Manages Damages Reduction/increase according to shield

            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                
                bool electric = false;
                if (null != damageInfo && self.shield > 0)
                {
                    //Debug.Log("DamageInfo not null");
                    if (damageInfo.attacker != null && null != damageInfo.inflictor && null != damageInfo.inflictor.name && damageInfo.inflictor.name != "FireTrail(Clone)" && damageInfo.inflictor.name != "BeetleQueenAcid(Clone)" && damageInfo.inflictor.name != "DotController(Clone)" && (damageInfo.inflictor.name == "LightningStake(Clone)" ||
                    /*damageInfo.procChainMask.mask == 8 ||*/
                    damageInfo.inflictor.name == "VagrantCannon(Clone)" ||
                    damageInfo.inflictor.name == "VagrantTrackingBomb(Clone)" ||
                    damageInfo.attacker.name == "VagrantBody(Clone)" ||
                    damageInfo.attacker.name == "JellyfishBody(Clone)" ||
                    damageInfo.inflictor.name == "BeamSphere(Clone)" ||
                    damageInfo.inflictor.name == "ElectricOrbProjectile(Clone)" ||
                    damageInfo.inflictor.name == "ElectricWormBody(Clone)" ||
                    (damageInfo.inflictor.name != "ToolbotBody(Clone)" && damageInfo.inflictor.name != "CryoCanisterBombletsProjectile(Clone)" && damageInfo.inflictor.name != "CryoCanisterProjectile(Clone)" && damageInfo.inflictor.name != "CommandoBody(Clone)" && damageInfo.damageType == DamageType.Stun1s)) ||
                    null == damageInfo.inflictor && damageInfo.damageType == DamageType.Stun1s)
                    {
                        electric = true;
                        //Debug.Log("Attack is electric");
                    }
                    //Debug.Log("Damage info : ");
                    //Debug.Log("Attacker : " + damageInfo.attacker);
                    //Debug.Log("Inflictor name : " + damageInfo.inflictor);
                    //Debug.Log("DamageType : " + damageInfo.damageType);
                    //Debug.Log("Damage input : " + damageInfo.damage);
                    //Debug.Log("ProcChainMask : " + damageInfo.procChainMask.mask);

                    if (!electric)
                        {
                            //Debug.Log("Damage reduced by shield");
                            float damageleftaftershieldreduction = damageInfo.damage * damageReduction - self.shield;

                            if (damageleftaftershieldreduction > 0)
                            {
                                damageInfo.damage = self.shield + damageleftaftershieldreduction / damageReduction;
                            }
                            else
                            {
                                damageInfo.damage = damageInfo.damage * damageReduction;
                            }
                        }
                        else
                        {
                        //Debug.Log("Damage increased by shield");
                        float damageleftaftershieldincrease = damageInfo.damage * damageIncrease - self.shield;

                            if (damageleftaftershieldincrease > 0)
                            {
                                damageInfo.damage = self.shield + damageleftaftershieldincrease / damageIncrease;
                            }
                            else
                            {
                                damageInfo.damage = damageInfo.damage * damageIncrease;
                            }
                        }
                        orig(self, damageInfo);
                    //Debug.Log("Damage output : " + damageInfo.damage);
                    //Debug.Log("");


                }
                else
                {
                    orig(self, damageInfo);
                }
            };
        }
    }
}