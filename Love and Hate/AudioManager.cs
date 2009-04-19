using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Love_and_Hate
{
    public class AudioManager
    {
        private ContentManager mContentMgr;
        private bool mInitialized = false;

        private static AudioManager mInstance = new AudioManager();
        private static Dictionary<String, SoundBank> mSounds = new Dictionary<string, SoundBank>();
        private static Dictionary<String, WaveBank> mWaves = new Dictionary<string, WaveBank>();

        static String SOUND_BANK_CONTENT = "\\audio\\Sound Bank.xsb";

        static public AudioManager Instance
        {
            get
            {
                return mInstance;
            }
        }

        private AudioEngine mAudioEngine;

        public AudioEngine Engine
        {
            get { return mAudioEngine; }
        }

        public AudioManager()
        {

        }

        public void Load(ContentManager contentMgr, String asset)
        {
            mContentMgr = contentMgr;
            mAudioEngine = new AudioEngine(contentMgr.RootDirectory + asset);
        }
        
        public void Update()
        {
            mAudioEngine.Update();
        }

        public void PlaySound(String cue)
        {
            SoundBank sb = null;           

            if (mSounds.ContainsKey(SOUND_BANK_CONTENT))
                sb = mSounds[SOUND_BANK_CONTENT];
            else
            {
                String WAVE_BANK_CONTENT = "\\audio\\Wave Bank.xwb";

                // Instantiate a wave bank to be able to play a sound but we don't need a
                // wave bank past that.
                //
                mWaves.Add(WAVE_BANK_CONTENT, new WaveBank(Engine, mContentMgr.RootDirectory + WAVE_BANK_CONTENT));

                sb = new SoundBank(Engine, mContentMgr.RootDirectory + SOUND_BANK_CONTENT);
                
                mSounds.Add(SOUND_BANK_CONTENT, sb);
            }

            sb.PlayCue(cue);
        }

        public void StopSound(String cue)
        {
            if (mSounds.ContainsKey(SOUND_BANK_CONTENT))
                mSounds[SOUND_BANK_CONTENT].GetCue(cue).Stop(AudioStopOptions.AsAuthored);
        }

        public bool IsSoundPlaying(String cue)
        {
            if (mSounds.ContainsKey(SOUND_BANK_CONTENT))
                return mSounds[SOUND_BANK_CONTENT].GetCue(cue).IsPlaying;

            return false;
        }

        public void PlayMusic(String asset)
        {
            //WaveBank wb = null;

            //if (mWaves.ContainsKey(asset))
            //{
            //    wb = mWaves[asset];
            //}
            //else
            //    wb = new WaveBank(Instance.Engine, asset);

            //wb.

            //sb.PlayCue(cue);
        }
    }
}
