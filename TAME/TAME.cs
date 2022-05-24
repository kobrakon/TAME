using BepInEx;
using UnityEngine;

namespace AmbientMusic
{
    [BepInPlugin("com.kobrakon.tame", "TAME", "1.0.0")]
    public class MusicEnginePlugin : BaseUnityPlugin
    {
        public GameObject Hook;

        private void Awake()
        {
            Hook = new GameObject("TAME"); // create hook
            Hook.AddComponent<UpdateHandler>(); // attach mono script
            DontDestroyOnLoad(Hook); // add hook

            Logger.LogInfo($"Tarkov Ambient Music Engine (TAME): Initializing");
        }
    }
}