using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Utilities
{
    [System.Serializable]
    public struct TPAudioObject
    {
        public string Name;
        public AudioClip Clip;
    }

    public class TPAudioPool
    {
        public static Dictionary<string, TPAudioBundle> Pool = new Dictionary<string, TPAudioBundle>();
        static AudioSource _source;
        static AudioSource _themeSource;

        public static AudioSource Source
        {
            get
            {
                if (_source == null)
                {
                    _source = GetSource("TPAudioSource", false);
                }

                return _source;
            }
            set
            {
                _source = value;
                if (!ReferenceEquals(_source, null))
                {
                    _source = GetSource("TPAudioSource", false);
                    CopySource(ref _source, value);
                }
            }
        }

        public static AudioSource ThemeSource
        {
            get
            {
                if (_themeSource == null)
                {
                    _themeSource = GetSource("TPAudioThemeSource", true);
                }

                return _themeSource;
            }
            set
            {
                _themeSource = value;
                if (!ReferenceEquals(_themeSource, null))
                {
                    _themeSource = GetSource("TPAudioThemeSource", true);
                    CopySource(ref _themeSource, value);
                }
            }
        }

        static AudioSource GetSource(string Name, bool loop)
        {
            GameObject obj = GameObject.Find(Name);

            if (ReferenceEquals(obj, null))
            {
                return CreateNewSource(Name, loop);
            }
            return GameObject.Find(Name).GetComponent<AudioSource>();
        }

        static AudioSource CreateNewSource(string Name, bool loop)
        {
            GameObject newObj = new GameObject(Name, typeof(AudioSource));
            AudioSource audioSource = newObj.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = loop;

            Object.DontDestroyOnLoad(newObj);
            return audioSource;
        }

        static void CopySource(ref AudioSource copyFrom, AudioSource copyTo)
        {
            copyFrom.bypassEffects = copyTo.bypassEffects;
            copyFrom.bypassListenerEffects = copyTo.bypassListenerEffects;
            copyFrom.bypassReverbZones = copyTo.bypassReverbZones;
            copyFrom.dopplerLevel = copyTo.dopplerLevel;
            copyFrom.enabled = copyTo.enabled;
            copyFrom.hideFlags = copyTo.hideFlags;
            copyFrom.ignoreListenerPause = copyTo.ignoreListenerPause;
            copyFrom.ignoreListenerVolume = copyTo.ignoreListenerVolume;
            copyFrom.loop = copyTo.loop;
            copyFrom.maxDistance = copyTo.maxDistance;
            copyFrom.minDistance = copyTo.minDistance;
            copyFrom.mute = copyTo.mute;
            copyFrom.outputAudioMixerGroup = copyTo.outputAudioMixerGroup;
            copyFrom.panStereo = copyTo.panStereo;
            copyFrom.pitch = copyTo.pitch;
            copyFrom.playOnAwake = copyTo.playOnAwake;
            copyFrom.priority = copyTo.priority;
            copyFrom.reverbZoneMix = copyTo.reverbZoneMix;
            copyFrom.rolloffMode = copyTo.rolloffMode;
            copyFrom.spatialBlend = copyTo.spatialBlend;
            copyFrom.spatialize = copyTo.spatialize;
            copyFrom.spatializePostEffects = copyTo.spatializePostEffects;
            copyFrom.spread = copyTo.spread;
            copyFrom.tag = copyTo.tag;
            copyFrom.time = copyTo.time;
            copyFrom.timeSamples = copyTo.timeSamples;
            copyFrom.velocityUpdateMode = copyTo.velocityUpdateMode;
            copyFrom.volume = copyTo.volume;
            copyFrom.SetCustomCurve(AudioSourceCurveType.CustomRolloff, copyTo.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
            copyFrom.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, copyTo.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
            copyFrom.SetCustomCurve(AudioSourceCurveType.SpatialBlend, copyTo.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
            copyFrom.SetCustomCurve(AudioSourceCurveType.Spread, copyTo.GetCustomCurve(AudioSourceCurveType.Spread));
        }


        /// <summary>  
        ///  Adds AudioBundle to pool.
        /// </summary> 
        public static void AddToPool(string bundleName, TPAudioBundle bundle)
        {
            Pool[bundleName] = bundle;
        }

        /// <summary>  
        ///  Adds AudioBundle to pool.
        /// </summary> 
        public static void AddToPool(TPAudioBundle bundle)
        {
            AddToPool(bundle.name, bundle);
        }

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetSource(TPAudioBundle bundle, string audioName)
        {
            Source.clip = GetClip(bundle, audioName);
            return Source;
        }

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetSource(string bundleName, string audioName)
        {
            return SetSource(Pool[bundleName], audioName);
        }

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetTheme(TPAudioBundle bundle, string audioName)
        {
            ThemeSource.clip = GetClip(bundle, audioName);
            return ThemeSource;
        }

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetTheme(string bundleName, string audioName)
        {
            return SetTheme(Pool[bundleName], audioName);
        }

        /// <summary>
        /// Gets clip by name in bundle.
        /// </summary>
        public static AudioClip GetClip(TPAudioBundle bundle, string audioName)
        {
            foreach (var obj in bundle.AudioObjects)
            {
                if (obj.Name == audioName)
                {
                    return obj.Clip;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets clip by name in bundle which has name.
        /// </summary>
        public static AudioClip GetClip(string bundleName, string audioName)
        {
            return GetClip(Pool[bundleName], audioName);
        }

    }

}
