using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgPotionController : MedicineBase
{
    [SerializeField] float _dmgBoost;
    public override void onEndUse()
    {
        _isUse = false;

        _player.setDmg(_player._baseDmg - _dmgBoost);
        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {
        _isUse = true;
        _player.setDmg(_player._baseDmg + _dmgBoost);
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
