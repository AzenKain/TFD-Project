using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : WeaponBase
{
    [Header("---EnemyShooter---")]
    [SerializeField] BulletBase _bullet;
    [SerializeField] Transform _spawn;
    [SerializeField] float _scope;

    public override void attack()
    {
        if (_tmpTimeUnitDelay > 0)
            return;

        if (_enemy.GetPositionPlayer() == null)
            return;

        BulletBase bulletInstant = ObjectPooling.Instant.getComp<BulletBase>(_bullet);

        if (bulletInstant == null) Debug.Log("Null");
        bulletInstant.transform.position = this._spawn.position;
        bulletInstant.Init(5f, _damage + _enemy.getDmg(), 2, this._scope, (_enemy.GetPositionPlayer() - (Vector2)this.transform.position).normalized);
        if (bulletInstant.gameObject.activeSelf == false)
        {
            bulletInstant.gameObject.SetActive(true);
        }
        this._tmpTimeUnitDelay = this._timeUnitDelay;
        this._isAttack = true;
    }

    public override bool onEndAttack()
    {
        this._isAttack = false;
        return false;
    }

    public override bool onStartAttack()
    {
        return false;
    }

    public override void selectDirection()
    {
        return;
    }


}
