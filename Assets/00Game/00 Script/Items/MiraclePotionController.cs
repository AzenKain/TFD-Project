using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraclePotionController : MedicineBase
{
    [SerializeField] GhostFX _effect;
    [SerializeField] float _dodgeRateBoost;
    [SerializeField] float _speedBoost;
    private void Start()
    {
        this._effect = GetComponent<GhostFX>();
    }
    public override void onEndUse()
    {
        _isUse = false;
    
        this._player.setSpeed(this._player._speed - _speedBoost);
        this._player.setDodge(this._player._dodgeRate - _dodgeRateBoost);
        _effect.endGhostFX();
        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {
        _isUse = true;
        this._player.setSpeed(this._player._speed + _speedBoost);
        this._player.setDodge(this._player._dodgeRate + _dodgeRateBoost);
        _effect.Init(this._player.GetComponentInChildren<SpriteRenderer>());
        _effect.startGhostFX();
    }

    public override void use()
    {
        if (_tmpTimeUnitDelay > 0)
            return;

        if (_isUse)
            return;
        SoundManager.Instant.PlaySound("glass-bottle-break");
        _tmpTimeUnitDelay = _timeUnitDelay;

        routineUseEffect = StartCoroutine(useEffect());
    }


}
