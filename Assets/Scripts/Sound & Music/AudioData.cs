using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public struct AudioData
{
    [SerializeField] public AudioResource audioResource;
    [SerializeField] public AudioMixerGroup audioMixerGroup;
}
