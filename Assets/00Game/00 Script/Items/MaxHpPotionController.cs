using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpPotionController : MedicineBase
{
    [SerializeField] float _maxHpBoost;
    public override void onEndUse()
    {
        _isUse = false;

        _player.setMaxHp(_player._maxHp - _maxHpBoost);
        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {
        _isUse = true;
        _player.setMaxHp(_player._maxHp + _maxHpBoost);
    }

    public override void use()
    {
        if (_tmpTimeUnitDelay > 0)
            return;

        if (_isUse)
            return;

        _tmpTimeUnitDelay = _timeUnitDelay;
        SoundManager.Instant.PlaySound("glass-bottle-break");
        routineUseEffect = StartCoroutine(useEffect());
    }


}
