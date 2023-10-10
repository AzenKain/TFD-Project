using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurWeaponController : WeaponBase
{
    [Header("---MinotaurWeapon---")]
    [SerializeField] Attacking _attacking;
    Coroutine routineSlash;
    Coroutine routineSweep;

    public override void attack()
    {
        if (this._isAttack)
            return;

        if (_tmpTimeUnitDelay > 0)
        {
            return;
        }
        this._tmpTimeUnitDelay = this._timeUnitDelay;

        int random = Random.Range(0, 2);
        Debug.Log(random);
        if (random == 0)
        {

            this.routineSweep = StartCoroutine(processSweep());
        }
        else
        {
            this.routineSlash = StartCoroutine(processSlash());
        }

    }

    IEnumerator processSlash()
    {
        this._isAttack = true;
        _enemy.setBossState(BossState.PreSlash);
        yield return new WaitForSeconds(0.4f);
        _enemy.setBossState(BossState.Slash);
        this._attacking._isAttack = true;
        yield return new WaitForSeconds(0.7f);
        this._isAttack = false;
        this._attacking._isAttack = _isAttack;
        _enemy.setBossState(BossState.Run);
        StopCoroutine(routineSlash);
    }
    IEnumerator processSweep()
    {
        this._isAttack = true;
        for (int i = 0; i < 2; i++)
        {
            _enemy.setBossState(BossState.PreSweep);
            for (int j = 0; i < 3; i++)
            {
                Vector2 dict = (_enemy.GetPositionPlayer() - (Vector2)_enemy.transform.position).normalized * -500f;
                _enemy.AddForceEnmemy(dict);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.4f);
            _enemy.setBossState(BossState.Sweep);
            this._attacking._isAttack = true;
            for (int j = 0; i < 4; i++)
            {
                Vector2 dict = (_enemy.GetPositionPlayer() - (Vector2)_enemy.transform.position).normalized * 500f;
                _enemy.AddForceEnmemy(dict);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.7f);
            this._attacking._isAttack = false;
            this._isAttack = false;
            _enemy.setBossState(BossState.Run);
        }

        StopCoroutine(routineSweep);
    }
    public override bool onEndAttack()
    {
        return false;
    }

    public override bool onStartAttack()
    {
        if (this._isAttack)
            return false;

        this._tmpTimeUnitDelay -= Time.deltaTime;
        return false;
    }

    public override void selectDirection()
    {
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        this._attacking = GetComponentInChildren<Attacking>();
        this._attacking._isAttack = _isAttack;
        this._attacking.setDng(_enemy.getDmg());
    }


}
