using BepInEx;
using RoR2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;

namespace BetterMedkit
{
    [BepInDependency("com.bepis.r2api")]

    [BepInPlugin("com.Squiddle.bettermedkit", "BetterMedkit", "1.0.0")]

    public class BetterMedkit : BaseUnityPlugin
    {
        public void Awake()
        {

        }
    }
}