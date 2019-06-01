using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;


namespace GestureFix
{
    [BepInDependency("com.bepis.r2api")]
    
    [BepInPlugin("com.Squiddle.GestureFix", "Gesture is red", "1.0")]
    public class GestureFix : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect += orig => { };

            IL.RoR2.ItemCatalog.DefineItems += (il) =>
            {
                var c = new ILCursor(il);

                c.GotoNext(
                    x => x.MatchLdstr("Prefabs/PickupModels/PickupFossil"),
                    x => x.MatchStfld<RoR2.ItemDef>("pickupModelPath")
                );
                c.Index -= 3;
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);

            };
            
            MethodInfo defineItems = typeof(ItemCatalog).GetMethod("DefineItems", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            defineItems.Invoke(null, null);
            Debug.Log("Gesture is red loaded");
        }

    }
}
