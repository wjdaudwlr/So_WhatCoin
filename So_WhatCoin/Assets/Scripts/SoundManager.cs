using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound;

    public AudioClip[] bgClips;

    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        BgSoundPlay(0);
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();

        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(int clip)
    {
        if (bgSound.isPlaying)
            bgSound.Stop();
        bgSound.clip = bgClips[clip];
        bgSound.loop = true;
        bgSound.volume = 0.3f;
        bgSound.Play();
    }
}
