using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{

    [SerializeField] Toggle _fullScreen;
    [SerializeField] Slider _BG;
    [SerializeField] Slider _SFX;
    float _BGValue, _SFXValue;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init()
    {
        string data = PlayerPrefs.GetString(CONSTANT.nameDataVolume);
        var dataParsed = JSON.Parse(data);
        this._BGValue = dataParsed["BG"].AsFloat;
        this._SFXValue = dataParsed["SFX"].AsFloat;
    }
    public void UpdateDataOption()
    {
        string data = PlayerPrefs.GetString(CONSTANT.nameDataVolume);
        var dataParsed = JSON.Parse(data);
        this._BGValue = dataParsed["BG"].AsFloat;
        this._SFXValue = dataParsed["SFX"].AsFloat;
        this._BG.value = this._BGValue;
        this._SFX.value = this._SFXValue;
        this._fullScreen.isOn = Screen.fullScreen;
    }
    // Update is called once per frame
    void Update()
    {
        this.updateVolumeData();
    }
    void updateVolumeData()
    {
        if (this.gameObject.activeSelf == false)
        {
            return;
        }


        if (this._BGValue == this._BG.value && this._SFXValue == this._SFX.value)
            return;

        string data = PlayerPrefs.GetString(CONSTANT.nameDataVolume);
        var dataParsed = JSON.Parse(data);
        dataParsed["BG"] = this._BG.value;
        dataParsed["SFX"] = this._SFX.value;
        PlayerPrefs.SetString(CONSTANT.nameDataVolume, dataParsed.ToString());
        this._BGValue =  this._BG.value;
        this._SFXValue = this._SFX.value;
        AsyncController.Instant.updateAudioSource();
    }
    public KeyValuePair<float, float> getValueVolume()
    {
        return new KeyValuePair<float, float>(_BGValue, _SFXValue);
    }
    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen; 
    }
    public void Exit()
    {
        this.gameObject.SetActive(false);
    }
}
