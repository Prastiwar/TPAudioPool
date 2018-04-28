/**
*   Authored by Tomasz Piowczyk
*   @ 2018 MIT LICENSE
*   Repository: https://github.com/Prastiwar/TPAudioPool 
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Utilities
{
    [Serializable]
    public struct TPAudioObject
    {
        public string Name;
        public AudioClip Clip;
    }

    public class TPAudioPool
    {
        static Dictionary<string, TPAudioBundle> Pool = new Dictionary<string, TPAudioBundle>();
        static AudioSource _source;
        static AudioSource _themeSource;

        public enum Sources
        {
            Source,
            Theme
        }

        /// <summary>
        /// Gets AudioSource from GameObject named "TPAudioSource"
        /// <para> On Get - returns source - if null, create it </para>
        /// <para> On Set - behaves like preset </para>
        /// </summary>
        public static AudioSource Source {
            get {
                if (_source == null)
                {
                    _source = GetOrCreateSource("TPAudioSource", false);
                }

                return _source;
            }
            set {
                _source = value;
                if (!ReferenceEquals(_source, null))
                {
                    _source = GetOrCreateSource("TPAudioSource", false);
                    CopySource(ref _source, value);
                }
            }
        }

        /// <summary>
        /// Gets AudioSource from GameObject named "TPAudioThemeSource"
        /// <para> On Get - returns source - if null, create it </para>
        /// <para> On Set - behaves like preset </para>
        /// </summary>
        public static AudioSource ThemeSource {
            get {
                if (_themeSource == null)
                {
                    _themeSource = GetOrCreateSource("TPAudioThemeSource", true);
                }
                return _themeSource;
            }
            set {
                _themeSource = value;
                if (!ReferenceEquals(_themeSource, null))
                {
                    _themeSource = GetOrCreateSource("TPAudioThemeSource", true);
                    CopySource(ref _themeSource, value);
                }
            }
        }

        /// <summary>  
        ///  Adds Audio Bundle to pool.
        /// </summary> 
        public static void AddToPool(string bundleName, TPAudioBundle bundle)
        {
            Pool[bundleName] = bundle;
        }
        /// <summary>  
        ///  Adds Audio Bundle to pool.
        /// </summary> 
        public static void AddToPool(TPAudioBundle bundle)
        {
            AddToPool(bundle.name, bundle);
        }

        /// <summary>  
        ///  Removes Audio Bundle from pool.
        /// </summary> 
        public static void RemoveFromPool(string bundleName)
        {
            Pool.Remove(bundleName);
        }
        /// <summary>  
        ///  Removes Audio Bundle from pool.
        /// </summary> 
        public static void RemoveFromPool(TPAudioBundle bundle)
        {
            Pool.Remove(bundle.name);
        }
        /// <summary>  
        ///  Removes all Audio Bundles from pool.
        /// </summary> 
        public static void Dispose()
        {
            Pool.Clear();
        }

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetClip(TPAudioBundle bundle, string audioName, Sources source = Sources.Source)
        {
            AudioClip clip = GetClip(bundle, audioName);
            if (source == Sources.Source)
            {
                Source.clip = clip;
            }
            else
            {
                ThemeSource.clip = clip;
            }
            return Source;
        }
        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetClip(string bundleName, string audioName, Sources source = Sources.Source)
        {
            return SetClip(GetBundle(bundleName), audioName, source);
        }

#if NET_2_0 || NET_2_0_SUBSET
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        /// <param name="mono">Monobehaviour script which calls this method</param>
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, MonoBehaviour mono, Action onAudioEnd, float volumeScale = 1.0f)
        {
            AudioClip clip = GetClip(bundle, audioName);
            Source.PlayOneShot(clip, volumeScale);
            mono.StartCoroutine(DelayAction(clip.length, onAudioEnd));
        }
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        /// <param name="mono">Monobehaviour script which calls this method</param>
        public static void PlayOneShot(string bundleName, string audioName, MonoBehaviour mono, Action onAudioEnd, float volumeScale = 1.0f)
        {
            PlayOneShot(GetBundle(bundleName), audioName, mono, onAudioEnd, volumeScale);
        }
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale = 1.0f)
        {
            AudioClip clip = GetClip(bundle, audioName);
            Source.PlayOneShot(clip, volumeScale);
        }
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(string bundleName, string audioName, float volumeScale = 1.0f)
        {
            PlayOneShot(GetBundle(bundleName), audioName, volumeScale);
        }
#else
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale = 1.0f, Action onAudioEnd = null)
        {
            AudioClip clip = GetClip(bundle, audioName);
            Source.PlayOneShot(clip, volumeScale);
            DelayAction(clip.length, onAudioEnd);
        }
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(string bundleName, string audioName, float volumeScale = 1.0f, Action onAudioEnd = null)
        {
            PlayOneShot(GetBundle(bundleName), audioName, volumeScale, onAudioEnd);
        }
#endif

#if NET_2_0 || NET_2_0_SUBSET
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        /// <param name="mono">Monobehaviour script which calls this method</param>
        public static void Play(TPAudioBundle bundle, string audioName, MonoBehaviour mono, Action onAudioEnd, Sources source = Sources.Source, ulong delay = 0)
        {
            AudioClip clip = GetClip(bundle, audioName);
            if (source == Sources.Source)
            {
                Source.clip = clip;
                Source.Play(delay);
            }
            else
            {
                ThemeSource.clip = clip;
                ThemeSource.Play(delay);
            }
            mono.StartCoroutine(DelayAction(clip.length + delay, onAudioEnd));
        }
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        /// <param name="mono">Monobehaviour script which calls this method</param>
        public static void Play(string bundleName, string audioName, MonoBehaviour mono, Action onAudioEnd, Sources source = Sources.Source, ulong delay = 0)
        {
            Play(GetBundle(bundleName), audioName, mono, onAudioEnd, source, delay);
        }
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        public static void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.Source, ulong delay = 0)
        {
            AudioClip clip = GetClip(bundle, audioName);
            if (source == Sources.Source)
            {
                Source.clip = clip;
                Source.Play(delay);
            }
            else
            {
                ThemeSource.clip = clip;
                ThemeSource.Play(delay);
            }
        }
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        public static void Play(string bundleName, string audioName, Sources source = Sources.Source, ulong delay = 0)
        {
            Play(GetBundle(bundleName), audioName, source, delay);
        }
#else
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        public static void Play(TPAudioBundle bundle, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null)
        {
            AudioClip clip = GetClip(bundle, audioName);

            if (source == Sources.Source)
            {
                Source.clip = clip;
                Source.Play(delay);
            }
            else
            {
                ThemeSource.clip = clip;
                ThemeSource.Play(delay);
            }

            DelayAction(clip.length + delay, onAudioEnd);
        }
        /// <summary>
        /// Sets clip to theme or source and calls Play.
        /// </summary>
        public static void Play(string bundleName, string audioName, Sources source = Sources.Source, ulong delay = 0, Action onAudioEnd = null)
        {
            Play(GetBundle(bundleName), audioName, source, delay, onAudioEnd);
        }
#endif

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
            return GetClip(GetBundle(bundleName), audioName);
        }

        /// <summary>
        /// Gets bundle by name.
        /// </summary>
        public static TPAudioBundle GetBundle(string bundleName)
        {
            return Pool[bundleName];
        }

#if NET_2_0 || NET_2_0_SUBSET
        private static IEnumerator DelayAction(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
#else
        private static async void DelayAction(float delay, Action action)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(delay));
            action();
        }
#endif
        private static AudioSource GetOrCreateSource(string Name, bool loop)
        {
            GameObject obj = GameObject.Find(Name);

            if (ReferenceEquals(obj, null))
            {
                return CreateNewSource(Name, loop);
            }
            return obj.GetComponent<AudioSource>();
        }

        private static AudioSource CreateNewSource(string Name, bool loop)
        {
            GameObject newObj = new GameObject(Name, typeof(AudioSource));
            AudioSource audioSource = newObj.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            UnityEngine.Object.DontDestroyOnLoad(newObj);

            return audioSource;
        }

        private static void CopySource(ref AudioSource copyFrom, AudioSource copyTo)
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

    }

}
