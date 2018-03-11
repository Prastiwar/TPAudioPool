using System.Collections.Generic;
using UnityEngine;

namespace TP.Utilities
{
    [CreateAssetMenu(menuName = "TPAudioPool/Audio Bundle", fileName = "AudioBundle")]
    public class TPAudioBundle : ScriptableObject
    {
        public TPAudioObject[] AudioObjects;
    }
}
