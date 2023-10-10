using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling _instant;
    public static ObjectPooling Instant => _instant;

    //pooling
    Dictionary<GameObject, List<GameObject>> _pool = new Dictionary<GameObject, List<GameObject>>();



    private void Awake()
    {
        _instant = this;
    }

    public virtual GameObject getObj(GameObject prefab)
    {
        if (_pool.ContainsKey(prefab))
        {
            foreach (GameObject g in _pool[prefab])
            {
                if (g.activeSelf)
                    continue;
                return g;
            }
        }
        else
        {
            _pool.Add(prefab, new List<GameObject>());
        }

        GameObject g2 = Instantiate(prefab, this.transform.position, Quaternion.identity);
        _pool[prefab].Add(g2);

        return g2;
    }

    public virtual T getComp<T>(T prefab) where T : MonoBehaviour
    {

        return this.getObj(prefab.gameObject).GetComponent<T>();
    }
}