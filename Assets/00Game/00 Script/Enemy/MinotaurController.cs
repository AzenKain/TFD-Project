using Pathfinding;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossState
{
    Run,
    PreSlash,
    Slash,
    PreSweep,
    Sweep,
    Death
}
public class MinotaurController : EnemyController
{
    protected override void updateAni()
    {
       
        _animController.UpdateAnim(_bossState);
    }
    public override void Init()
    {
        base.Init();
        this._bossState = BossState.Run;
    }
    protected override void updateState()
    {

        if (this._movement != Vector2.zero)
        {
            int animationIndex = 0;
            float angle = Mathf.Atan2(this._movement.y, this._movement.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }
            if (angle >= 0 && angle < 90 || angle > 270 && angle <= 360) {
                this._colli.offset = new Vector2(this._colli.offset.x, this._colli.offset.y);
                animationIndex = 1;
            }
            else if (angle >= 90 && angle <= 270) {
                this._colli.offset = new Vector2(-this._colli.offset.x, this._colli.offset.y);
                animationIndex = 0;
            };
            this._animController.UpdateValidateAmin(CONSTANT.stateEnemy, animationIndex);

        }

    }

    protected override void CaculatePath()
    {
        if (this._isBackToPossition)
        {
            return;
        }

        if (this._player == null || Vector2.Distance(this.transform.position, this._pastPosition) > 20f)
        {
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector2 spawnPosition = this._pastPosition + randomOffset;
            this.CurrentWayPoint = 0;
            this._seeker.StartPath(this.transform.position, spawnPosition, OnPathComplate);
            this._isBackToPossition = true;
            this.setHp(this._maxHp);
            this._colli.isTrigger = true;
            return;
        }

        if (_seeker.IsDone())
        {
            this.CurrentWayPoint = 0;
            this._seeker.StartPath(this.transform.position, this._player.transform.position, OnPathComplate);
        }
    }

    protected override void OnPathComplate(Path p)
    {
        if (p.error)
            return;
        this._path = p;
        this.MoveToTarger();
    }

    protected override void MoveToTarger()
    {
        this._movement = Vector2.zero;
        if (this._path == null) return;

        if (CurrentWayPoint >= this._path.vectorPath.Count - 1)
        {
            this._isBackToPossition = false;
            this._colli.isTrigger = false;
            return;
        }

        this._movement = ((Vector2)this._path.vectorPath[CurrentWayPoint] - (Vector2)this.transform.position).normalized;
        if (Vector2.Distance(this.transform.position, this._path.vectorPath[CurrentWayPoint]) < 0.3f)
        {
            CurrentWayPoint++;
        }
    }
    protected override void OnDead()
    {
        base.OnDead();

        this._bossState = BossState.Death;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
