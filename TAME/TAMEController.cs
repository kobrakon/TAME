using System.IO;
using UnityEngine;
using System.Timers;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace AmbientMusic
{
    public class TAMEController
    {
        private static Timer timer;
        private static List<AudioClip> MusicList = new List<AudioClip>();
        private static List<AudioClip> NDEMusicList = new List<AudioClip>();
        public static string AmbientMusicPath = BepInEx.Paths.PluginPath + "TAME\\Music\\Ambience";
        public static string NDEMusicPath = BepInEx.Paths.PluginPath + "TAME\\Music\\NDE";
        private static AudioSource playerAudioSource;
        private static bool _timerInstanceActive = false;
        public static int musicVolume { get; set; }

        public static void StartAmbienceTimer()
        {
            if (_timerInstanceActive) return;

            timer = new Timer(Random.Range(600000, 720000));
            timer.Enabled = true;
            timer.Elapsed += (sender, e) => PlayAmbientMusic();
            timer.Start();
            _timerInstanceActive = true;
            timer.AutoReset = true;
        }

        public static void RevokeMusic()
        {
            timer.Stop();
            timer.Enabled = false;

            if (playerAudioSource == null) return;
            playerAudioSource.Stop();
            playerAudioSource.clip = null;
            _timerInstanceActive = false;
        }

        // stop audioplayer and dispose of active timer instance
        public static void EndMusic() { playerAudioSource.Stop(); playerAudioSource.clip = null; timer.Dispose(); _timerInstanceActive = false; }

        public static void PlayAmbientMusic()
        {
            var comp = UpdateHandler.player.GetComponent<AudioSource>();

            if (comp == null) UpdateHandler.player.AddComponent<AudioSource>();

            playerAudioSource = comp;

            if (playerAudioSource.isPlaying) return;

            playerAudioSource.clip = MusicList.RandomElement();
            playerAudioSource.Play(0);

            while (playerAudioSource.isPlaying)
            {
                playerAudioSource.volume = musicVolume;
                if (!playerAudioSource.isPlaying) { EndMusic(); break; }
                continue;
            }
        }

        public static async void RequestClips(string path, AudioType audioType, List<AudioClip> list)
        {
            using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
            {
                var request = req.SendWebRequest();

                while (!request.isDone)
                {
                    await request.Await();
                }

                if (req.isNetworkError || req.isHttpError) { Plugin.logger.LogError($"Failed to retrieve file from {path}"); return; }

                AudioClip clip = DownloadHandlerAudioClip.GetContent(req);
                list.Add(clip);
            }
        }

        public static void GetAudio()
        {
            string[] files = Directory.GetFiles(AmbientMusicPath);

            foreach (var file in files)
            {
                string url = "file:///" + file.Replace("\\", "/");

                if (file.Contains(".wav")) { RequestClips(url, AudioType.WAV, MusicList); }
                else if (file.Contains(".mp3")) { RequestClips(url, AudioType.MPEG, MusicList);  }
                else if (file.Contains("ogg")) { RequestClips(url, AudioType.OGGVORBIS, MusicList); }
                RequestClips(url, AudioType.UNKNOWN, MusicList);
            }

            string[] ndeFiles = Directory.GetFiles(NDEMusicPath);

            foreach (var file in ndeFiles)
            {
                string url = "file:///" + file.Replace("\\", "/");

                if (file.Contains(".wav")) { RequestClips(url, AudioType.WAV, MusicList); }
                else if (file.Contains(".mp3")) { RequestClips(url, AudioType.MPEG, MusicList); }
                else if (file.Contains("ogg")) { RequestClips(url, AudioType.OGGVORBIS, MusicList); }
                RequestClips(url, AudioType.UNKNOWN, MusicList);
            }
        }
    }
}
