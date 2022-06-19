using System.IO;
using System.Media;
using System.Timers;

namespace AmbientMusic
{
    public class TAMEController
    {
        private static SoundPlayer soundPlayer; // yeah yeah stinky soundplayer what the fuck ever
        public static Timer timer;
        public static bool isPlaying { get; set; } = false;
        public static bool TimerRunning { get; set; } = false;
        public static bool eventIsRunning { get; set; } = false;

        public static void AmbienceTimer()
        {
            timer = new Timer(Random.Range(300000, 600000));
            timer.Start();
            TimerRunning = true;

            timer.Elapsed += (sender, e) => PlayMusicTime(); // okay so literally nothing uses the sender and e args but the timer is a little bitch and refuses to run the event without them

            timer.AutoReset = true;

            timer.Enabled = true;
        }

        public static void PlayBigHurtAudio()
        {
            SoundPlayer("\\BepInEx\\plugins\\TAME\\Music\\NDE");

            isPlaying = true;
            eventIsRunning = true;

            soundPlayer.PlaySync();
        }

        public static void PlayMusicTime()
        {
            if (!UpdateHandler.IsPlayerBigHurt())
            {
                SoundPlayer("\\BepInEx\\plugins\\TAME\\Music\\Ambience");

                isPlaying = true;

                soundPlayer.PlaySync();

                isPlaying = false;
                AmbienceTimer();
            }
            return;
        }

        public static void ShutUp()
        {
            soundPlayer.Stop();
            timer.Stop();

            isPlaying = false;
        }

        private static SoundPlayer SoundPlayer(string fullpath)
        {
            var path = Directory.GetFiles(Directory.GetCurrentDirectory() + fullpath, "*.wav");
            int choices = Random.Range(0, path.Length);

            soundPlayer = new SoundPlayer
            {
                SoundLocation = path[choices]
            };

            return soundPlayer;
        }
    }
}
