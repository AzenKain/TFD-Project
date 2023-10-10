using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee: WeaponBase
{
    [Header("---EnemyMelee---")]
    [SerializeField] Attacking _attacking;
    Coroutine routineAttack;

    // Start is called before the first frame update
    void Start()
    {
        this._attacking = GetComponentInChildren<Attacking>();
        this._attacking._isAttack = _isAttack;
        this._attacking.setDng(_enemy.getDmg());
    }
    public override void attack()
    {
        if (_tmpTimeUnitDelay > 0)
        {
            return;
        }
        this._tmpTimeUnitDelay = this._timeUnitDelay;
        this.routineAttack = StartCoroutine(processAttack());
        
    }
    IEnumerator processAttack()
    {
        this._isAttack = true;
        this._attacking._isAttack = _isAttack;
        _enemy.setEnemyState(EnemyState.Attack);
        yield return new WaitForSeconds(0.5f);
        this._isAttack = false;
        this._attacking._isAttack = _isAttack;
        _enemy.setEnemyState(EnemyState.Run);
        StopCoroutine(routineAttack);
    }
    public override bool onEndAttack()
    {
        return false;
    }

    public override bool onStartAttack()
    {
        this._tmpTimeUnitDelay -= Time.deltaTime;
        return false;
    }

    public override void selectDirection()
    {
        return;
    }

}
