using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.GameManager
{
    public class SoundManager
    {
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundManager();
                }
                return _instance;
            }
            private set { }
        }

        private SoundPlayer[] sounds;
        private readonly string path = @"./Music\\";

        private SoundManager()
        {
            sounds = new SoundPlayer[3];

            sounds[0] = new SoundPlayer(path + "Main.wav");
            sounds[1] = new SoundPlayer(path + "Battle.wav");
            sounds[2] = new SoundPlayer(path + "Shop.wav");
        }

        public void StartMusic(MusicType music)
        {
            switch (music)
            {
                case MusicType.Main:
                    sounds[0].PlayLooping(); break;
                case MusicType.Battle:
                    sounds[1].PlayLooping(); break;
                case MusicType.Shop:
                    sounds[2].PlayLooping(); break;
            }
        }

        public void StopMusic()
        {
            foreach (var sound in sounds) { sound.Stop(); }
        }

        public enum MusicType
        {
            Main,
            Battle,
            Shop
        }

    }
}
