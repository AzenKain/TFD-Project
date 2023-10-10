using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManger : Singleton<EnemyManger>
{
    [SerializeField] List<EnemyController> _enemyPrefab = new List<EnemyController>();
    public List<EnemyController> enemyPrefab =>_enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    EnemyController GetEnemy(int _id)
    {
        for (int i = 0; i < _enemyPrefab.Count; i++)
        {
            if (_enemyPrefab[i].getDataEnemy()._ID == _id)
            {
                return _enemyPrefab[i];
            }
        }
        return null;
    }
    public List<EnemyController> SpawmEnemy(string data)
    {
        List<EnemyController> _res = new List<EnemyController>();
        var dataParsed = JSON.Parse(data);

        for (int i = 0; i < dataParsed.Count; i++)
        {
            int enemyDataID = dataParsed[i]["ID"].AsInt;

            EnemyController enemyChoice = this.GetEnemy(enemyDataID);
            if (enemyChoice == null)
            {
                continue;
            }

            for (int j = 0; j < dataParsed[i]["quantity"].AsInt; j++)
            {
                EnemyController enemyInstant = ObjectPooling.Instant.getComp<EnemyController>(enemyChoice);
                _res.Add(enemyInstant);
            }

        }
        return _res;
    }
}
