using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPotionController : MedicineBase
{
    [SerializeField] float _armorBoost;
    public override void onEndUse()
    {
        _isUse = false;

        _player.setArmor(_player._armor - _armorBoost);
        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {

        _isUse = true;
        _player.setArmor(_player._armor + _armorBoost);
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
