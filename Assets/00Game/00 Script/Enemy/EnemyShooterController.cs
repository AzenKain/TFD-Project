using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : EnemyController
{
    [SerializeField] float _distanceToPlayer;
    protected override void updateAni()
    {
        _animController.UpdateAnim(_enemyState);
    }

    protected override void updateState()
    {
        int animationIndex = 0;
        if (this._movement != Vector2.zero)
        {

            float angle = Mathf.Atan2(this._movement.y, this._movement.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360;
            }
            if (angle > 60 && angle < 120) animationIndex = 1;
            else if (angle > 0 && angle < 60) animationIndex = 2;
            else if (angle > 300 && angle < 360) animationIndex = 2;
            else if (angle > 240 && angle < 300) animationIndex = 3;
            else if (angle > 180 && angle < 240) animationIndex = 4;
            else if (angle > 120 && angle < 280) animationIndex = 4;
        }

        this._animController.UpdateValidateAmin(CONSTANT.stateEnemy, animationIndex);
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
            Vector2 randomOffset = (Random.insideUnitCircle * Random.Range(10f, 50f)).normalized;
            Vector2 post = (Vector2)this._player.transform.position + randomOffset * _distanceToPlayer;
            this._seeker.StartPath(this.transform.position, post, OnPathComplate);
        }
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
        this.weaponRotation.dict = this._movement;
        if (Vector2.Distance(this.transform.position, this._path.vectorPath[CurrentWayPoint]) < 0.3f)
        {
            CurrentWayPoint++;
        }
    }

    protected override void OnPathComplate(Path p)
    {
        if (p.error)
            return;
        this._path = p;
        this.MoveToTarger();
    }
}
