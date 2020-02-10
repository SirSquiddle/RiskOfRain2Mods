using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using System;

namespace MMTGA
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.makemonstertoothgreatagain", "MakeMonsterToothGreatAgain", "2.1.1")]

    public class MMTGA : BaseUnityPlugin
    {
        public int stacksPerOrb = 7;
        public float healingStack1 = 0.045f;
        public float healingOtherStacks = 0.0225f;
        public object instr2 = null;
        public void Awake()
        {
            IL.RoR2.GlobalEventManager.OnCharacterDeath += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdloc(32)
                    );
                instr2 = c.Next.Operand;
                
                c.Index += 1;

                c.EmitDelegate<Func<int, int>>((toothcount) =>
                {
                    toothcount = 0;
                    System.Collections.ObjectModel.ReadOnlyCollection<TeamComponent> teammembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
                    for (int i = 0; i < teammembers.Count; i++)
                    {
                        if (Util.LookUpBodyNetworkUser(teammembers[i].gameObject))
                        {
                            CharacterBody component = teammembers[i].GetComponent<CharacterBody>();
                            if (component && component.inventory)
                            {
                                toothcount += component.inventory.GetItemCount(ItemIndex.Tooth);
                            }
                        }
                    }
                    Debug.Log(toothcount);
                    return toothcount;
                });

                c.GotoNext(
                    x => x.MatchLdloc(32)
                    );

                c.Index += 1;

                c.EmitDelegate<Func<int, int>>((toothcount) =>
                {
                    toothcount = 0;
                    System.Collections.ObjectModel.ReadOnlyCollection<TeamComponent> teammembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
                    for (int i = 0; i < teammembers.Count; i++)
                    {
                        if (Util.LookUpBodyNetworkUser(teammembers[i].gameObject))
                        {
                            CharacterBody component = teammembers[i].GetComponent<CharacterBody>();
                            if (component && component.inventory)
                            {
                                toothcount += component.inventory.GetItemCount(ItemIndex.Tooth);
                            }
                        }
                    }
                    Debug.Log(toothcount);
                    return toothcount;
                });

                c.GotoNext(
                    x => x.MatchLdloc(52),
                    x => x.MatchLdloc(52),
                    x => x.MatchLdloc(52)
                    );
                c.Index += 5;
                c.Remove();
                c.Emit(OpCodes.Ldloc_S, instr2);
                c.EmitDelegate<Action<GameObject, int>>((healthOrbGameObject, toothCount ) =>
                {
                    float toothamount = 0;
                    System.Collections.ObjectModel.ReadOnlyCollection<TeamComponent> teammembers = TeamComponent.GetTeamMembers(TeamIndex.Player);
                    for(int i = 0; i < teammembers.Count; i++)
                    {
                        if (Util.LookUpBodyNetworkUser(teammembers[i].gameObject))
                        {
                            CharacterBody component = teammembers[i].GetComponent<CharacterBody>();
                            if (component && component.inventory)
                            {
                                toothamount += component.inventory.GetItemCount(ItemIndex.Tooth);
                            }
                        }
                    }

                    Debug.Log("Total tooth amount : "+toothamount);

                    healthOrbGameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                    healthOrbGameObject.GetComponentInChildren<HealthPickup>().flatHealing = 0f;
                    healthOrbGameObject.GetComponentInChildren<HealthPickup>().fractionalHealing = healingStack1 + healingOtherStacks * (toothamount - 1);
                    UnityEngine.Networking.NetworkServer.Spawn(healthOrbGameObject);
                    
                });
                //Debug.Log(il.ToString());
            };
        }
    }
}