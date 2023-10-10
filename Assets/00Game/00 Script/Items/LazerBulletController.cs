using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBulletController : BulletBase
{
    [SerializeField] float _timeStun;
    [SerializeField] string _tagAvoid;
    protected override void Boom(GameObject target)
    {

        IGetHit isCanGetHit = target.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;

        isCanGetHit.GetHit(this._dmg);

        IGetStun isCanGetStun = target.GetComponent<IGetStun>();
        if (isCanGetStun == null)
            return;
        isCanGetStun.GetStun(_timeStun);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
            return;
        if (collision.gameObject.CompareTag("Weapon"))
            return;
        if (_tagAvoid != null || _tagAvoid != "")
            if (collision.gameObject.CompareTag(_tagAvoid))
                return;
        this.Boom(collision.gameObject);
    }
}
