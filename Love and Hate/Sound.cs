using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Love_and_Hate
{
    class Sound
    {
        //waveBank.pl
        //soundBank.PlayCue("Explosion");

        static Dictionary<String, SoundBank> mSounds = new Dictionary<string, SoundBank>();

        static public void Load(String asset)
        {
            //if (!mSounds.ContainsKey(asset))
              //  mSounds[asset] = new SoundBank(AudioCore.Instance.Engine, "Content/audio/Sound Bank.xsb");
        }

        static public void PlaySound(String asset, String cueName)
        {
            if (!mSounds.ContainsKey(asset))
                mSounds[asset] = new SoundBank(AudioCore.Instance.Engine, asset);

            mSounds[asset].PlayCue(cueName);                
        }

        static public void PlayMusic(String asset)
        {

        }
    }
}
