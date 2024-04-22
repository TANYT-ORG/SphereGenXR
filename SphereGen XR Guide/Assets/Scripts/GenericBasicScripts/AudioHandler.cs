using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereGen.GuideXR
{
    [Serializable]
    public class SerializableAudioClipDictionary
    {
        public string key;
        public AudioClip clip;
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioHandler : MonoBehaviour
    {
        public static AudioHandler Instance { get; private set; }

        [SerializeField]
        private List<SerializableAudioClipDictionary> audioClips = new List<SerializableAudioClipDictionary>();

        private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

        private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();

            // Build the audioClipDictionary from the serialized audioClips list
            foreach (var audioClipEntry in audioClips)
            {
                if (!audioClipDictionary.ContainsKey(audioClipEntry.key))
                {
                    audioClipDictionary.Add(audioClipEntry.key, audioClipEntry.clip);
                }
            }
        }

        public bool PlayAudio(string audioKey)
        {
            if (audioClipDictionary.ContainsKey(audioKey))
            {
                audioSource.PlayOneShot(audioClipDictionary[audioKey]);
                return true;
            }

            return false;
        }

        private bool AddAudioClip(string audioKey, AudioClip audioClip)
        {
            if (audioClip == null)
            {
                Debug.LogWarning("Cannot add null audio clip.");
                return false;
            }

            if (audioClipDictionary.ContainsKey(audioKey))
            {
                Debug.LogWarning("Audio clip key already exists in the dictionary.");
                return false;
            }

            audioClipDictionary.Add(audioKey, audioClip);

            // Update the serialized audioClips list
            audioClips.Add(new SerializableAudioClipDictionary { key = audioKey, clip = audioClip });

            return true;
        }
    }
}
