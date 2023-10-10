using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SwordController : WeaponBase
{
    [SerializeField] Attacking _attacking;
    Vector3 mousePosition;
    void Start()
    {
        this._attacking = GetComponentInChildren<Attacking>();
        this._attacking._isAttack = _isAttack;
        this._attacking.setDng(_damage + _player._baseDmg);
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

            if ((angle >= 157.5 && angle <= 180)
            || (angle >= 202.5 && angle <= 225)
            || (angle >= 247.5 && angle <= 270)
            || (angle >= 292.5 && angle <= 315)
            || (angle >= 337.5 && angle <= 360)
            || (angle >= 22.5 && angle <= 45)
            || (angle >= 67.5 && angle <= 90)
            || (angle >= 112.5 && angle <= 135))
            {
                this._animator.SetFloat(CONSTANT.stateAttack, 1);
            }
            else
            {
                this._animator.SetFloat(CONSTANT.stateAttack, 0);
            }

            if ((angle >= 0 && angle <= 90) || (angle >= 270 && angle <= 360))
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this.transform.localRotation = targetRotation;
                this._uiWeapon.localPosition = new Vector2(0.3f, 0.1f);
                this._spriteRenderer.flipY = false;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiWeapon.localRotation = targetRotationUi;
            }

            else if (angle >= 90 && angle <= 270)
            {
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
                this.transform.localRotation = targetRotation;
                this._uiWeapon.localPosition = new Vector2(0.3f, -0.1f);
                this._spriteRenderer.flipY = true;
                Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                this._uiWeapon.localRotation = targetRotationUi;
            }
        }

    }

    public override void attack()
    {

        if (_tmpTimeUnitDelay > 0)
            return;
        SoundManager.Instant.PlaySound(this._nameMusicSFX);
        Vector3 shoulderToMouseDir = mousePosition - this.transform.position;
        shoulderToMouseDir.z = 0;
        float angle = Mathf.Atan2(shoulderToMouseDir.y, shoulderToMouseDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
        this._uiWeapon.localPosition = new Vector2(0.6f, -0.2f);
        Quaternion targetRotationUi = Quaternion.Euler(new Vector3(0f, 0f, -90f));
        this._uiWeapon.localRotation = targetRotationUi;
        this._spriteRenderer.flipY = false;
        _animator.SetTrigger("isAttack");

        this._tmpTimeUnitDelay = this._timeUnitDelay;
        this._isAttack = true;
        this._attacking._isAttack = _isAttack;

    }

    public override bool onEndAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {

            this._isAttack = false;
            this._attacking._isAttack = _isAttack;
            return true; 
        }
        return false;
    }

    public override bool onStartAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("AttackBase"))
        {
            this._isAttack = true;
            this._attacking._isAttack = _isAttack;
            return true; 
        }
        return false; 
    }


}


