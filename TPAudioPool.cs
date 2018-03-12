﻿using System.Collections;
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
            return obj.GetComponent<AudioSource>();
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
        ///  Adds Audio Bundle to pool.
        /// </summary> 
        public static void AddToPool(string bundleName, TPAudioBundle bundle) => Pool[bundleName] = bundle;
        /// <summary>  
        ///  Adds Audio Bundle to pool.
        /// </summary> 
        public static void AddToPool(TPAudioBundle bundle) => AddToPool(bundle.name, bundle);

        /// <summary>  
        ///  Removes Audio Bundle from pool.
        /// </summary> 
        public static void RemoveFromPool(string bundleName) => Pool.Remove(bundleName);
        /// <summary>  
        ///  Removes Audio Bundle from pool.
        /// </summary> 
        public static void RemoveFromPool(TPAudioBundle bundle) => Pool.Remove(bundle.name);
        /// <summary>  
        ///  Removes all Audio Bundles from pool.
        /// </summary> 
        public static void Dispose() => Pool.Clear();

        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetClipSource(TPAudioBundle bundle, string audioName) { Source.clip = GetClip(bundle, audioName); return Source; }
        /// <summary>
        /// Sets clip and returns AudioSource.
        /// </summary>
        public static AudioSource SetClipSource(string bundleName, string audioName) => SetClipSource(Pool[bundleName], audioName);


        /// <summary>
        /// Sets clip and returns Theme AudioSource.
        /// </summary>
        public static AudioSource SetClipTheme(TPAudioBundle bundle, string audioName) { ThemeSource.clip = GetClip(bundle, audioName); return ThemeSource; }
        /// <summary>
        /// Sets clip and returns Theme AudioSource.
        /// </summary>
        public static AudioSource SetClipTheme(string bundleName, string audioName) => SetClipTheme(Pool[bundleName], audioName);

        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(string bundleName, string audioName, float volumeScale) => Source.PlayOneShot(GetClip(bundleName, audioName), volumeScale);
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(TPAudioBundle bundle, string audioName, float volumeScale) => Source.PlayOneShot(GetClip(bundle, audioName), volumeScale);
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(string bundleName, string audioName) => Source.PlayOneShot(GetClip(bundleName, audioName));
        /// <summary> 
        /// Sets clip to Source then calls PlayOneShot. 
        /// </summary>
        public static void PlayOneShot(TPAudioBundle bundle, string audioName) => Source.PlayOneShot(GetClip(bundle, audioName));

        /// <summary>
        /// Sets clip to theme and calls Play.
        /// </summary>
        public static void PlayTheme(TPAudioBundle bundle, string audioName) { ThemeSource.clip = GetClip(bundle, audioName); ThemeSource.Play(); }
        /// <summary>
        /// Sets clip to theme and calls Play.
        /// </summary>
        public static void PlayTheme(TPAudioBundle bundle, string audioName, ulong delay) { ThemeSource.clip = GetClip(bundle, audioName); ThemeSource.Play(delay); }
        /// <summary>
        /// Sets clip to theme and calls Play.
        /// </summary>
        public static void PlayTheme(string bundleName, string audioName) { ThemeSource.clip = GetClip(bundleName, audioName); ThemeSource.Play(); }
        /// <summary>
        /// Sets clip to theme and calls Play.
        /// </summary>
        public static void PlayTheme(string bundleName, string audioName, ulong delay) { ThemeSource.clip = GetClip(bundleName, audioName); ThemeSource.Play(delay); }

        /// <summary>
        /// Sets clip to source and calls Play.
        /// </summary>
        public static void PlaySource(TPAudioBundle bundle, string audioName) { Source.clip = GetClip(bundle, audioName); Source.Play(); }
        /// <summary>
        /// Sets clip to source and calls Play.
        /// </summary>
        public static void PlaySource(TPAudioBundle bundle, string audioName, ulong delay) { Source.clip = GetClip(bundle, audioName); Source.Play(delay); }
        /// <summary>
        /// Sets clip to source and calls Play.
        /// </summary>
        public static void PlaySource(string bundleName, string audioName) { Source.clip = GetClip(bundleName, audioName); Source.Play(); }
        /// <summary>
        /// Sets clip to source and calls Play.
        /// </summary>
        public static void PlaySource(string bundleName, string audioName, ulong delay) { Source.clip = GetClip(bundleName, audioName); Source.Play(delay); }

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
        public static AudioClip GetClip(string bundleName, string audioName) => GetClip(Pool[bundleName], audioName);

        /// <summary>
        /// Gets bundle by name.
        /// </summary>
        public static TPAudioBundle GetBundle(string bundleName) => Pool[bundleName];

    }

}
