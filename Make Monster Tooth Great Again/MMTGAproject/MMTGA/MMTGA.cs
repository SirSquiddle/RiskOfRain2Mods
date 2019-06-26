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

    [BepInPlugin("com.Squiddle.makemonstertoothgreatagain", "MakeMonsterToothGreatAgain", "1.0.2")]

    public class MMTGA : BaseUnityPlugin
    {
        public int stacksPerOrb = 7;
        public float healingStack1 = 0.045f;
        public float healingOtherStacks = 0.0225f;
        public object instr2 = null;
        public void Awake()
        {
            On.RoR2.GravitatePickup.Start += (orig, self) =>
            {
               
                orig(self);
                self.rigidbody.AddForce(UnityEngine.Random.Range(-2500f, 2500f), 0, UnityEngine.Random.Range(-2500f, 2500f));
                
            };
            

            IL.RoR2.GlobalEventManager.OnCharacterDeath += (il) =>
            {
                var c = new ILCursor(il);
                c.GotoNext(
                    x => x.MatchLdloc(28)
                    );
                instr2 = c.Next.Operand;
                c.GotoNext(
                    x => x.MatchLdloc(49),
                    x => x.MatchLdloc(49),
                    x => x.MatchLdloc(49)
                    );
                c.Index += 5;
                c.Remove();
                c.Emit(OpCodes.Ldloc_S, instr2);
                c.EmitDelegate<Action<GameObject, int>>((healthOrbGameObject, toothCount ) =>
                {
                    Debug.Log("sqdknqlfn,lks,dlk,qsdlk,qsld,qslk,dqls,lk,dlq,sldk,sqld,lq,dlkqs,dlksdl,sdk,");
                    float num6 = Mathf.Pow((float)toothCount, 0.25f);
                    healthOrbGameObject.GetComponentInChildren<HealthPickup>().flatHealing = 0f;
                    healthOrbGameObject.GetComponentInChildren<HealthPickup>().fractionalHealing = healingStack1 + healingOtherStacks * (toothCount-1);
                    UnityEngine.Networking.NetworkServer.Spawn(healthOrbGameObject);
                    for (int i = stacksPerOrb; i <= toothCount; i += stacksPerOrb) 
                    {
                        GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/HealPack"), gameObject.transform.position, UnityEngine.Random.rotation);
                        gameObject5.GetComponent<TeamFilter>().teamIndex = healthOrbGameObject.GetComponent<TeamFilter>().teamIndex;
                        gameObject5.GetComponentInChildren<HealthPickup>().flatHealing = 0f;
                        gameObject5.GetComponentInChildren<HealthPickup>().fractionalHealing = healingStack1 + healingOtherStacks * (toothCount - 1);
                        gameObject5.transform.localScale = new Vector3(num6, num6, num6);
                        gameObject5.transform.position = new Vector3(healthOrbGameObject.transform.position.x + UnityEngine.Random.Range(-1f, 1f), healthOrbGameObject.transform.position.y + UnityEngine.Random.Range(-0.5f, 1.5f), healthOrbGameObject.transform.position.z + UnityEngine.Random.Range(-1f, 1f));
                        
                        UnityEngine.Networking.NetworkServer.Spawn(gameObject5);
                    }
                });
                //Debug.Log(il.ToString());
            };
        }
        
    }
}