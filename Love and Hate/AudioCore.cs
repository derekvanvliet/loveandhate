using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Love_and_Hate
{
    public class AudioCore
    {
        private static AudioCore mInstance = new AudioCore();

        static public AudioCore Instance
        {
            get { return mInstance; }
        }

        public AudioEngine Engine
        {
            get { return mAudioEngine; }
        }

        public AudioCore()
        {

        }

        private bool mInitialized = false;

        private AudioEngine mAudioEngine;

        public void Initialize()
        {
            //mAudioEngine = new AudioEngine("Content/audio/LoveAndHate.xgs");
        }

        public void Update()
        {
            //if (!mInitialized)
            //{
            //    Initialize();
            //    mInitialized = true;
            //}

            //mAudioEngine.Update();
        }
    }
}
