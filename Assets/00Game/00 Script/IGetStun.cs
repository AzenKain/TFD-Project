using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetStun
{
    public void GetStun(float timeStun);


    public IEnumerator processStun(float timeStun);
}
