using System.IO;
using UnityEngine;
using System.Media;
using System.Timers;


namespace AmbientMusic
{
    public class TAMEController
    {
        private static SoundPlayer soundPlayer; // yeah yeah stinky soundplayer what the fuck ever
        public static Timer timer;
        public static bool isPlaying = false;
        public static bool TimerRunning = false;
        public static bool eventIsRunning = false;

        public static void AmbienceTimer()
        {
            timer = new Timer(Random.Range(300000, 600000)); // pick random time between 5 to 10 minutes
            timer.Start(); // start timer
            TimerRunning = true;

            timer.Elapsed += (sender, e) => PlayMusicTime(); // okay so literally nothing uses the sender and e args but the timer is a little bitch and refuses to run the event without them

            timer.AutoReset = true; // reset

            timer.Enabled = true;
        }

        public static void PlayMusicTime() // plays music according to the time interval
        {
            var ipbh = UpdateHandler.IsPlayerBigHurt(); // get player near death bool and declare it

            if (!ipbh) // if player isn't near dead
            {
                SoundPlayer("\\BepInEx\\plugins\\TAME\\Music\\Ambience"); // get normal ambience

                isPlaying = true; // let everyone know music is playing

                soundPlayer.PlaySync(); // play sync so the method waits until music is done playing

                isPlaying = false; // let everyone know music's done
                AmbienceTimer(); // reset timer
            } else
            { // if player near dead
                 SoundPlayer("\\BepInEx\\plugins\\TAME\\Music\\NDE"); // get near death event music

                 isPlaying = true;
                 eventIsRunning = true;

                 soundPlayer.PlaySync();

                 isPlaying = false;
                 eventIsRunning = false;

                 AmbienceTimer();
            }
        }

        public static void ShutUp()
        {
            soundPlayer.Stop();
            timer.Stop();

            isPlaying = false;
        }

        private static SoundPlayer SoundPlayer(string fullpath)
        {
            var path = Directory.GetFiles(Directory.GetCurrentDirectory() + fullpath, "*.wav"); // get the directory and path then search for wav files
            int choices = Random.Range(0, path.Length); // pick random file

            soundPlayer = new SoundPlayer
            {
                SoundLocation = path[choices] // set soundlocation to the selected random file in the requested directory
            };

            return soundPlayer;
        }
    }
}
