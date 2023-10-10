using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotionController : MedicineBase
{
    [SerializeField] float _speedBoost;
    public override void onEndUse()
    {
        _isUse = false;
        _player.setSpeed(_player._speed - _speedBoost);
        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {
        _isUse = true;
        _player.setSpeed(_player._speed + _speedBoost);
    }

    public override void use()
    {
        if (_tmpTimeUnitDelay > 0)
            return;

        if (_isUse)
            return;

        _tmpTimeUnitDelay = _timeUnitDelay;

        routineUseEffect = StartCoroutine(useEffect());
    }

}
