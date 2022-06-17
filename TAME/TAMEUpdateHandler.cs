using EFT;
using UnityEngine;
using Comfort.Common;

namespace AmbientMusic
{
    public class UpdateHandler : MonoBehaviour
    {
        public static float current { get; private set; } // evil floating point number
        public GameWorld gameWorld = Singleton<GameWorld>.Instance;

        public void Update()
        {
            if (!Ready())
            {
                if (TAMEController.TimerRunning || TAMEController.isPlaying)
                {
                    TAMEController.timer.Stop();
                    TAMEController.ShutUp();
                    return;
                }
                return;
            }

            if (!TAMEController.TimerRunning)
                TAMEController.AmbienceTimer();

            if (IsPlayerBigHurt())
            {
                TAMEController.PlayBigHurtAudio();
            }

            current = gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
        }

        public bool IsPlayerHurt()
        {
            if (current != 0)
            {
                if (HealthDiff(current) >= 50 && !TAMEController.eventIsRunning)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool IsPlayerBigHurt()
        {
            if (current < 180)
            {
                return true;

            }
            return false;
        }

        bool Ready()
        {
            if (gameWorld == null || gameWorld.AllPlayers == null || gameWorld.AllPlayers.Count == 0 || gameWorld.AllPlayers[0] is HideoutPlayer)
            {
                if (TAMEController.isPlaying)
                    TAMEController.ShutUp();

                return false;
            }
            return true;
        }

        float HealthDiff(float val)
        {
            return Mathf.Floor(val - gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current); // calculate health differece, round down to avoid evil floating point inaccuracies
        }
    }
}
