using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class QueenBeeWeaponController : WeaponBase
{
    [Header("---QueenBeeShooter---")]
    [SerializeField] BulletBase _bullet;
    [SerializeField] Transform _spawn;
    [SerializeField]
    float _scope;
    public override void attack()
    {
        if (this._tmpTimeUnitDelay > 0)
            return;

        if (this._isAttack)
            return;

        if (this._enemy.GetPositionPlayer() == null)
            return;

        this._tmpTimeUnitDelay = this._timeUnitDelay;
        this._isAttack = true;

        int random = Random.Range(0, 2);
        Debug.LogError(random);
        if (random == 0)
        {
            float spacing = 0.2f;

            for (int i = 0; i < 4; i++)
            {
                BulletBase bulletInstant = ObjectPooling.Instant.getComp<BulletBase>(_bullet);

                if (bulletInstant == null) Debug.Log("Null");

                Vector3 bulletPosition = this._spawn.position + Vector3.down * spacing * i;
                bulletInstant.transform.position = bulletPosition;

                bulletInstant.Init(5f, _damage + _enemy.getDmg(), 3, this._scope, (_enemy.GetPositionPlayer() - (Vector2)this.transform.position).normalized);

                if (bulletInstant.gameObject.activeSelf == false)
                {
                    bulletInstant.gameObject.SetActive(true);
                }
            }


        }
        else
        {
            float spreadAngle = Random.Range(15, 45);
            float angleStep = spreadAngle / (4 - 1);

            Vector2 initialDirection = (_enemy.GetPositionPlayer() - (Vector2)this.transform.position).normalized;

            float startAngle = spreadAngle / 2f;

            for (int i = 0; i < 4; i++)
            {
                float angle = startAngle - i * angleStep;

                Vector2 direction = Quaternion.Euler(0f, 0f, angle) * initialDirection;

                BulletBase bulletInstant = ObjectPooling.Instant.getComp<BulletBase>(_bullet);

                if (bulletInstant == null) Debug.Log("Null");
                bulletInstant.transform.position = this._spawn.position;
                bulletInstant.Init(5f, _damage + _enemy.getDmg(), 3, this._scope, direction);
                if (bulletInstant.gameObject.activeSelf == false)
                {
                    bulletInstant.gameObject.SetActive(true);
                }
                bulletInstant.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            }
        }

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
