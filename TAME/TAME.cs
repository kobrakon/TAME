using BepInEx;
using UnityEngine;
using BepInEx.Logging;

namespace AmbientMusic
{
    [BepInPlugin("com.kobrakon.tame", "TAME", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject Hook;
        public static ManualLogSource logger;

        private void Awake()
        {
            logger = Logger;
            Hook = new GameObject("TAME"); // create hook
            Hook.AddComponent<UpdateHandler>(); // attach mono script
            DontDestroyOnLoad(Hook); // add hook

            Logger.LogInfo($"Tarkov Ambient Music Engine (TAME): Initializing");
            TAMEController.GetAudio();
        }
    }
}
