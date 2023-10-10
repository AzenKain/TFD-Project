using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.U2D;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    protected Camera mainCamera;
    [SerializeField] public Collider2D _colliBullet;
    [SerializeField] public Animator _animator;
    [SerializeField] public float _speed, _dmg, _lifeTime, _scope, _forceKnockBack;
    [SerializeField] protected Rigidbody2D _rigi;
    protected Vector2 _movement = Vector2.zero;
    protected Vector3 _pastPost = Vector2.zero;
    protected Coroutine routineAutoDestructAfter;
    protected TrailRenderer _trail;
    protected SpriteRenderer _spriteRenderer;
    Coroutine routineAutoDestruct;
    private void Awake()
    {
        _trail = this.GetComponent<TrailRenderer>();
    }

    void Start()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        this._colliBullet = this.GetComponent<Collider2D>();
        this._animator = this.GetComponent<Animator>();
        this._colliBullet.isTrigger = true;
        if (_rigi == null)
            _rigi = this.GetComponent<Rigidbody2D>();

    }
    public void Init(float speed, float dmg, float lifeTime, float scope,Vector2 movement)
    {
        this._speed = speed;
        this._dmg = dmg;
        this._lifeTime = lifeTime;
        this._movement = movement.normalized;
        this._scope = scope;
        this._trail.Clear();
        float angle = -(Mathf.Atan2(_movement.x, _movement.y) * Mathf.Rad2Deg) + 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = rotation;
        this._colliBullet.isTrigger = true;
        this._pastPost = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    protected virtual void OnEnable()
    {
        routineAutoDestruct = StartCoroutine(AutoDestruct());
    }
    protected IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(_lifeTime);
        this._animator.SetTrigger("Boom");
        routineAutoDestructAfter = StartCoroutine(AutoDestructAfterAnimaion());
    }
    protected IEnumerator AutoDestructAfterAnimaion()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        _rigi.velocity = _movement * _speed;

        if (Vector3.Magnitude(this.transform.position - this._pastPost) > _scope*_scope)
        {
            this._animator.SetTrigger("Boom");
            routineAutoDestructAfter = StartCoroutine(AutoDestructAfterAnimaion());
            this._movement = Vector3.zero;
            this._rigi.velocity = Vector3.zero;
        }
    }

    protected abstract void Boom(GameObject target);



    private void OnDisable()
    {
        _rigi.velocity = Vector2.zero;

        if (routineAutoDestruct != null)
            StopCoroutine(routineAutoDestruct);

        if (routineAutoDestructAfter != null)
            StopCoroutine(routineAutoDestructAfter);
    }
}
