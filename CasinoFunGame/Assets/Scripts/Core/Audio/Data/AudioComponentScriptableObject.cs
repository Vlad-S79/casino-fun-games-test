using System.Collections.Generic;
using Core.Common.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Audio.Data
{
    [CreateAssetMenu(fileName = "audio_component_scriptable_object", menuName = "Core/Audio Component Data", order = 1)]
    public class AudioComponentScriptableObject : ScriptableObject, IScriptableObjectInstaller
    {
        [SerializeField] private SerializedDictionary<string, AssetReferenceT<AudioClip>> _container;
        public Dictionary<string, AssetReferenceT<AudioClip>> Container { get; private set; }
        
        public void Init()
        {
            Container = _container.ToDictionary();
        }
    }
}