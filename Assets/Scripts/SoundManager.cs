using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private StringActionChannelISO soundClipChannel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip movementClip;
    [SerializeField] private AudioClip bottomClip;
    [SerializeField] private AudioClip rotationClip;
    [SerializeField] private AudioClip clearanceClip;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    void Start()
    {

        audioClips["movement"] = movementClip;
        audioClips["rotation"] = rotationClip;
        audioClips["bottom"] = bottomClip;
        audioClips["clearance"] = clearanceClip;

        soundClipChannel.MyEvent += PlayClip;
    }
    private void PlayClip(string clipName)
    {
        audioSource.PlayOneShot(audioClips[clipName]);
    }
}
