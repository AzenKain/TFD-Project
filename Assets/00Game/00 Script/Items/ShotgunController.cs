using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : WeaponBase
{
    [Header("---Shotgun---")]
    [SerializeField] int _numBullets = 5;
    Vector3 mousePosition;
    [SerializeField] BulletBase _bullet;
    [SerializeField] Transform _spawn;
    [SerializeField] float _scope;
    public override void attack()
    {
        if (_tmpTimeUnitDelay > 0)
            return;
        SoundManager.Instant.PlaySound(this._nameMusicSFX);
        float spreadAngle = Random.Range(15, 45);
        float angleStep = spreadAngle / (_numBullets - 1);

        Vector2 initialDirection = (this.mousePosition - this.transform.position).normalized;

        float startAngle = spreadAngle / 2f;

        for (int i = 0; i < _numBullets; i++)
        {
            float angle = startAngle - i * angleStep;

            // Create a new bullet object
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * initialDirection;

            BulletBase bulletInstant = ObjectPooling.Instant.getComp<BulletBase>(_bullet);

            if (bulletInstant == null) Debug.Log("Null");
            bulletInstant.transform.position = this._spawn.position;
            bulletInstant.Init(10f, _damage + _player._baseDmg, 2, this._scope, direction.normalized);
            if (bulletInstant.gameObject.activeSelf == false)
            {
                bulletInstant.gameObject.SetActive(true);
            }
            bulletInstant.transform.rotation = Quaternion.Euler(0f, 0f, angle);

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
        this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - transform.position).normalized;

        if (mouseDirection != Vector3.zero && this._isAttack == false)
        {
            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }

            if ((angle >= 0 && angle <= 90) || (angle >= 270 && angle <= 360))
            {
                this._uiWeapon.localPosition = new Vector2(0.6f, 0f);
                this._spriteRenderer.flipY = false;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiWeapon.localRotation = targetRotationUi;
            }

            else if (angle >= 90 && angle <= 270)
            {
                this._uiWeapon.localPosition = new Vector2(0.6f, 0f);
                this._spriteRenderer.flipY = true;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiWeapon.localRotation = targetRotationUi;
            }
        }
    }

}
