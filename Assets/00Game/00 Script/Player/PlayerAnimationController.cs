using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationController : AnimationControllerBase
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    [SerializeField] Material _materialBase;
    [SerializeField] Material _materialFlash;
    Coroutine routineGetHit;
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        this._spriteRenderer = this.GetComponent<SpriteRenderer>(); 
        this._materialBase = this._spriteRenderer.material;
    }

    private void Update()
    {

    }
    // Start is called before the first frame update
    public override void UpdateAnim(PlayerState playerState)
    {
        if (_animator == null) return;
        for (int i = 0; i <= (int)PlayerState.Slash; i++)
        {
            if ((int)playerState == i)
            {
                _animator.SetTrigger(playerState.ToString());
            }
        }
    }
    public override void UpdateValidateAmin(string name, float value)
    {
        if (_animator == null) return;
        _animator.SetFloat(name, value);
    }


    IEnumerator getHitIE()
    {
        this._spriteRenderer.material = this._materialFlash;
        yield return new WaitForSeconds(0.15f);
        this._spriteRenderer.material = this._materialBase;
        StopCoroutine(this.routineGetHit);
    }

    public override void SetGetHit()
    {
        this.routineGetHit = StartCoroutine(getHitIE());
    }

    public override void UpdateAnim(EnemyState enemyState)
    {
        return;
    }

    public override void UpdateAnim(BossState BossState)
    {
        return;
    }
}
