using EFT;
using UnityEngine;
using Comfort.Common;

namespace AmbientMusic
{
    public class UpdateHandler : MonoBehaviour
    {
        private static float current = 0;

        public void Update() // base unity mono method that checks and updates values every frame
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            var r = Ready();
            

            if (!r)
            {
                if (TAMEController.TimerRunning || TAMEController.isPlaying) // if session isn't ready and either the timer or music is active
                {
                    TAMEController.timer.Stop(); // stop timer
                    TAMEController.ShutUp(); // stop music
                }
                return;
            }
            
            var iph = IsPlayerHurt();
            
            if (!TAMEController.TimerRunning) // if timer isn't running
                TAMEController.AmbienceTimer(); // run timer

            if (iph) // if player is hurt
            {
                TAMEController.ShutUp(); // mute music
            }

            current = gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current; // update health value
        }

        public bool IsPlayerHurt() // check if player is hurt
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            if (current != 0)
            {
                var healthdiff = Mathf.Floor(current - gameWorld.AllPlayers[0].HealthController.GetBodyPartHealth(EBodyPart.Common).Current); // calculate health difference

                if (healthdiff >= 50 && !TAMEController.eventIsRunning) // if health difference is greater than or equal to 50, and the near death event hasn't occured
                {
                    return true; // return true
                }
                return false; // not hurt? return false
            }

            return false;
        }

        public static bool IsPlayerBigHurt()
        {
            if (current < 180) // if health is below 180
            {
                return true; // return true

            } else // not dying?
            {
                return false; // return false
            }
        }

        bool Ready()
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
