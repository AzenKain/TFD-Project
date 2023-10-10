using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBeeBulletController : BulletBase
{
    [SerializeField] string _tagAvoid;
    protected override void Boom(GameObject target)
    {

        IGetHit isCanGetHit = target.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;

        isCanGetHit.GetHit(this._dmg);

        IGetKnockBack isCanKnockBack = target.gameObject.GetComponent<IGetKnockBack>();

        if (isCanKnockBack == null)
            return;

        int percent = Random.Range(1, 3);
        Vector2 dict = new Vector2();
        if (percent == 1)
        {
            dict = (target.transform.position - this.transform.position).normalized;
        }
        else
        {
            dict = Vector2.zero;
        }

        isCanKnockBack.GetKnockBack(dict, _forceKnockBack);
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
