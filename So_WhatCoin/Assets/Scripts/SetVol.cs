using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVol : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetSFXVol (float sliderValue) 
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }

    public void SetBGVol(float sliderValue)
    {
        mixer.SetFloat("BGSound", Mathf.Log10(sliderValue) * 20);
    }
}
