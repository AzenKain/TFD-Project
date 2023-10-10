using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class InfoController : Singleton<InfoController> 
{
    [SerializeField] TMP_Text _info;
    [SerializeField] Text _gold;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
    }

   void UpdateInfo()
    {
        PlayerController _player = GameManager.Instant.player;
        string info = $"HP: {_player._hp}/{_player._maxHp}\n";
        info += $"Ammor: {_player._armor}\n";
        info += $"Speed: {_player._speed}\n";
        info += $"Damage: {_player._baseDmg} \n";
        info += $"Dodge: {_player._dodgeRate * 100}%";
        this._info.text = info;
        this._gold.text = $"{_player._gold}$";
    }
    
}
