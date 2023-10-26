using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioListener))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    DestroySound Audio_Prefab = null;
    [SerializeField]
    AudioSource AudioBG;
    [SerializeField]
    List<DestroySound> AudioS = new List<DestroySound>();
    [SerializeField]
    List<AudioClip> SoundFiles = new List<AudioClip>();

    void Start(){
        Object[] file = Resources.LoadAll("Sound", typeof(AudioClip));
        foreach(Object o in file){
            SoundFiles.Add((AudioClip)o);
        }
        string data = PlayerPrefs.GetString(CONSTANT.nameDataVolume);
        var dataParsed = JSON.Parse(data);
        Audio_Prefab.Audio.volume = dataParsed["SFX"].AsFloat;
        AudioBG.volume = dataParsed["BG"].AsFloat;
    }

    public void PlaySound(string nameSound){
        foreach(AudioClip A in SoundFiles){
            if(A.name.ToLower() != nameSound.ToLower())
                continue;
            AudioSource source = GetAudioSource();

            source.clip = A;
            source.gameObject.SetActive(true);
        }
    }
    public void PlaySoudBG(AudioClip clip)
    {
        if (AudioBG.clip == clip)
        {
            return;
        }
        if (AudioBG.isPlaying)
        {
            AudioBG.Stop();
        }
        AudioBG.clip = clip;
        AudioBG.Play();
    }
    AudioSource GetAudioSource(){
        foreach(DestroySound D in AudioS){
            if(D.gameObject.activeSelf)
                continue;
            return D.Audio;
        }

        DestroySound D2 = Instantiate(Audio_Prefab,this.transform.position,Quaternion.identity,this.transform).GetComponent<DestroySound>();
        AudioS.Add(D2);
        D2.gameObject.SetActive(false);

        return D2.Audio;
    }
}
