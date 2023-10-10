using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotionController : MedicineBase
{
    [SerializeField] float _hpRestore;
    public override void onEndUse()
    {
        _isUse = false;

        StopCoroutine(routineUseEffect);
    }

    public override void onStartUse()
    {

        _isUse = true;
        _player.restoreHp(_hpRestore);
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
