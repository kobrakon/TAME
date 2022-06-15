using EFT;
using UnityEngine;
using Comfort.Common;

namespace AmbientMusic
{
    public class UpdateHandler : MonoBehaviour
    {
        public static float current { get; private set; }; // evil floating point number

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

            if (IsPlayerHurt())
            {
                TAMEController.ShutUp();
            }

            current = gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
        }

        public static bool IsPlayerHurt()
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            if (current != 0)
            {
                var healthdiff = Mathf.Floor(current - gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current); // calculate health difference

                if (healthdiff >= 50 && !TAMEController.eventIsRunning)
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

            } else // not dying?
            {
                return false;
            }
        }

        static bool Ready()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var sessionResultPanel = Singleton<SessionResultPanel>.Instance;

            // if the gameworld info doesn't exist, return false
            if (gameWorld == null || gameWorld.AllPlayers == null || gameWorld.AllPlayers.Count == 0 || gameWorld.AllPlayers[0] is HideoutPlayer || sessionResultPanel != null)
            {
                return false;
            } else
            {
                // if the gameworld is properly loaded, return true
                return true;
            }
        }
    }
}
