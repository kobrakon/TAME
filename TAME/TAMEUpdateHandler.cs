using EFT;
using UnityEngine;
using Comfort.Common;

namespace AmbientMusic
{
    public class UpdateHandler : MonoBehaviour
    {
        public static float current { get; private set; } // evil floating point number
        public GameWorld gameWorld;
        public static GameObject player;

        public void Update()
        {
            gameWorld = Singleton<GameWorld>.Instance;

            if (!Ready()) { TAMEController.RevokeMusic(); return; }

            player = GameObject.Find("PlayerSuperior(Clone)");
            current = gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
            TAMEController.StartAmbienceTimer();
        }

        bool IsPlayerHurt() => current == gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current ? return false : return true;
        bool IsPlayerBigHurt() => HealthDiff(current) >= 50 ? true : false;
        bool Ready() => Singleton<GameWorld>.Instantiated ? true : false;
        float HealthDiff(float val) => Mathf.Floor(val - gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current);
    }
}
