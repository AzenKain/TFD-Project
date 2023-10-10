using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetKnockBack 
{
    public void GetKnockBack(Vector2 knockBack, float force);


    public IEnumerator processKnockBack(Vector2 knockBack, float force);

}
