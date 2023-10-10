using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _speedAttack;
    [SerializeField] protected float _timeUnitDelay;
    protected float _tmpTimeUnitDelay;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Transform _uiWeapon;
    [SerializeField] protected string _nameMusicSFX;
    protected bool _isAttack = false;
    protected PlayerController _player;
    protected EnemyController _enemy;
    void Start()
    {

        this._animator = GetComponentInChildren<Animator>();

        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        this._tmpTimeUnitDelay = _timeUnitDelay;
    }
    public void Init(PlayerController player)
    {
        this._player = player;
    }

    public void Init(EnemyController enemy)
    {
        this._enemy = enemy;
    }
    // Update is called once per frame
    protected void Update()
    {
        if (onStartAttack())
        {
            return;
        }
        selectDirection();

        this._tmpTimeUnitDelay -= Time.deltaTime;

        if (onEndAttack())
        {
            return;
        }

    }

    public abstract void attack();
    public abstract void selectDirection();
    public abstract bool onEndAttack();
    public abstract bool onStartAttack();
}
