using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAnimation : AnimationControllerBase
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    [SerializeField] Material _materialBase;
    [SerializeField] Material _materialFlash;
    Coroutine routineGetHit;
    public override void SetGetHit()
    {
        this.routineGetHit = StartCoroutine(getHitIE());
    }

    public override void UpdateAnim(PlayerState playerState)
    {
        return;
    }

    public override void UpdateAnim(EnemyState enemyState)
    {
        return;
    }

    public override void UpdateAnim(BossState BossState)
    {
        if (_animator == null) return;
        if (BossState == BossState.PreSweep)
            Debug.LogError("trigger");
        for (int i = 0; i <= (int)BossState.Death; i++)
        {
            if ((int)BossState == i)
            {
                _animator.SetTrigger(BossState.ToString());
          
            }
        }
    }

    public override void UpdateValidateAmin(string name, float value)
    {
        if (_animator == null) return;
        _animator.SetFloat(name, value);
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        this._materialBase = this._spriteRenderer.material;
    }
    IEnumerator getHitIE()
    {
        this._spriteRenderer.material = this._materialFlash;
        yield return new WaitForSeconds(0.15f);
        this._spriteRenderer.material = this._materialBase;
        StopCoroutine(this.routineGetHit);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
