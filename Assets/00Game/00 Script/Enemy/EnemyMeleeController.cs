using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMeleeController : EnemyController
{
    // Start is called before the first frame update

    protected override void updateAni()
    {
        _animController.UpdateAnim(_enemyState);
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

            if (angle >= 35 && angle < 135) animationIndex = 0;
            else if (angle >= 0 && angle < 35) animationIndex = 1;
            else if (angle >= 315 && angle <= 360) animationIndex = 1;
            else if (angle >= 225 && angle < 315) animationIndex = 2;
            else if (angle >= 135 && angle < 225) animationIndex = 3;
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

        if (CurrentWayPoint >= this._path.vectorPath.Count-1)
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
}
