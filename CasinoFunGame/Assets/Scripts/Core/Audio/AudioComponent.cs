using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Audio.Data;
// using Game.Loader;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Core.Audio
{
    public class AudioComponent
    {
        private AudioComponentScriptableObject _audioComponentScriptableObject;
        
        private Dictionary<string, AudioClip> _dictionary;

        private const string AudioEffectsKey = "audio_effects_volume";
        private const string AudioMusicKey = "audio_music_volume";
        private const string AudioIsActiveKey = "is_active_audio";
        
        private AudioSource _musicAudioSource;
        private AudioSource[] _effectsAudioSource;
        
        private byte _currentEffectsAudioSourceIndex;
        private float _musicVolume, _effectsVolume;
        private bool _isActive;

        private const int AudioSourcesEffectCount = 5;
        
        [Inject]
        private void Init(AudioComponentScriptableObject audioComponentScriptableObject/*, LoaderComponent loaderComponent*/)
        {
            _dictionary = new Dictionary<string, AudioClip>();
            _audioComponentScriptableObject = audioComponentScriptableObject;
            
            LoadAudioClipDictionary(_audioComponentScriptableObject.Container);
            
            // var task = LoadAudioClipDictionaryAsync(_audioComponentScriptableObject.Container);
            // loaderComponent.AddLoadTask(task);

            InitAudioSources();
            LoadAudioPreset();
        }

        private void InitAudioSources()
        {
            var audioSourceContainer = new GameObject("audio");
            audioSourceContainer.AddComponent<AudioListener>();

            var musicAudioSourceObject = new GameObject("music_audio_source");
            musicAudioSourceObject.transform.SetParent(audioSourceContainer.transform);
            _musicAudioSource = musicAudioSourceObject.AddComponent<AudioSource>();
            _musicAudioSource.loop = true;
            _musicAudioSource.playOnAwake = false;

            _effectsAudioSource = new AudioSource[AudioSourcesEffectCount];
            for (int i = 0; i < AudioSourcesEffectCount; i++)
            {
                var effectAudioSourceObject = new GameObject("effect_audio_source_" + i);
                effectAudioSourceObject.transform.SetParent(audioSourceContainer.transform);
                _effectsAudioSource[i] = effectAudioSourceObject.AddComponent<AudioSource>();
                _effectsAudioSource[i].playOnAwake = false;
            }
            
            Object.DontDestroyOnLoad(audioSourceContainer);
        }

        private void LoadAudioPreset()
        {
            SetMusicVolume(PlayerPrefs.GetFloat(AudioMusicKey, 1));
            SetEffectVolume(PlayerPrefs.GetFloat(AudioEffectsKey, 1));
            _isActive = PlayerPrefs.GetInt(AudioIsActiveKey, 1) == 1;
        }

        public void LoadAudioClipDictionary(Dictionary<string, AssetReferenceT<AudioClip>> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                LoadAudioClip(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public async Task LoadAudioClipDictionaryAsync(Dictionary<string, AssetReferenceT<AudioClip>> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                await LoadAudioClipAsync(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        public void ReleaseAudioClipDictionaryAsync(Dictionary<string, AssetReferenceT<AudioClip>> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                ReleaseAudioClipAsync(keyValuePair.Key, keyValuePair.Value);
            }
        }
        
        public void LoadAudioClip(string key, AssetReferenceT<AudioClip> assetReferenceAudioClip)
        {
            if (_dictionary.ContainsKey(key))
            {
                Debug.LogError(" *** Audio["+ key +"] Already Contains Key: ");
                return;
            }

            if(CheckAssetReferenceAudioClipNull(key, assetReferenceAudioClip)) return;

            var handle = assetReferenceAudioClip.LoadAssetAsync();
            handle.WaitForCompletion();
                
            _dictionary.Add(key, handle.Result);
        }

        public async Task LoadAudioClipAsync(string key, AssetReferenceT<AudioClip> assetReferenceAudioClip)
        {
            if (_dictionary.ContainsKey(key))
            {
                Debug.LogError(" *** Audio["+ key +"] Already Contains Key: ");
                return;
            }

            if(CheckAssetReferenceAudioClipNull(key, assetReferenceAudioClip)) return;

            var task = assetReferenceAudioClip.LoadAssetAsync().Task;
            var result = await task;
                
            _dictionary.Add(key, result);
        }

        public void ReleaseAudioClipAsync(string key, AssetReferenceT<AudioClip> assetReferenceAudioClip)
        {
            if (!_dictionary.ContainsKey(key))
            {
                Debug.LogError(" *** Audio Dictionary NOT Have Key : " + key);
                return;
            }

            if(CheckAssetReferenceAudioClipNull(key, assetReferenceAudioClip)) return;

            _dictionary.Remove(key);
            assetReferenceAudioClip.ReleaseAsset();
        }

        private bool CheckAssetReferenceAudioClipNull(string key, AssetReferenceT<AudioClip> assetReferenceAudioClip)
        {
            if (assetReferenceAudioClip != null) return false;
            
            Debug.LogError(" *** Audio[" + key + "] value == null");
            return true;
        }
        
        
        public void SetActive(bool value)
        {
            if (_isActive) Stop();
            _isActive = value;
            
            PlayerPrefs.SetInt(AudioIsActiveKey, _isActive ? 1 : 0);
            PlayerPrefs.Save();
        }

        public bool IsActive() => _isActive;
         
        public void SetMusicVolume(float value)
        {
            if(_musicVolume == value) return;
            
            _musicVolume = value;
            _musicAudioSource.volume = _musicVolume;
            
            PlayerPrefs.SetFloat(AudioMusicKey, value);
            PlayerPrefs.Save();
        }
        public void SetEffectVolume(float value)
        {
            if(_effectsVolume == value) return;
            
            _effectsVolume = value;
            
            foreach (var audioSource in _effectsAudioSource)
            {
                audioSource.volume = _effectsVolume;
            }
            
            PlayerPrefs.SetFloat(AudioEffectsKey, value);
            PlayerPrefs.Save();
        }

        public float GetMusicVolume() => _musicVolume;
        public float GetEffectVolume() => _effectsVolume;

        public void Stop()
        {
            _musicAudioSource.Stop();
            
            foreach (var audioSource in _effectsAudioSource)
                audioSource.Stop();
        }

        public void Play(string audioId, float pitch = 1)
        {
            if(!_isActive) return;
            
            if (!_dictionary.TryGetValue(audioId, out var audioClip))
            {
              Debug.Log(" *** Not Contains " + audioId + " Audio Clip In Audio Container"); 
              return;
            }

            var audioSource = _effectsAudioSource[_currentEffectsAudioSourceIndex];
            
            audioSource.clip = audioClip;
            audioSource.pitch = pitch;
            audioSource.Play();

            _currentEffectsAudioSourceIndex++;
            if (_currentEffectsAudioSourceIndex >= _effectsAudioSource.Length)
            {
                _currentEffectsAudioSourceIndex = 0;
            }
        }

        public void PlayMusic(string audioId)
        {
            if(!_isActive) return;
            
            if (!_dictionary.TryGetValue(audioId, out var audioClip))
            {
                Debug.Log(" *** Not Contains " + audioId + " Audio Clip In Audio Container"); 
                return;
            }

            _musicAudioSource.clip = audioClip;
            _musicAudioSource.Play();
        }

        public void SetPitchMusic(float pitch) => _musicAudioSource.pitch = pitch;

        public void PauseMusic() => _musicAudioSource.Pause();
        public void UnPauseMusic() => _musicAudioSource.UnPause();
        
        public void PauseEffect()
        {
            foreach(var audioSource in _effectsAudioSource)
                audioSource.Pause();
        }
        
        public void UnPauseEffect()
        {
            foreach(var audioSource in _effectsAudioSource)
                audioSource.UnPause();
        }
    }
}