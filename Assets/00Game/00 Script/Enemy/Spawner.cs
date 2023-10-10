using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<EnemyController> _localEnemy = new List<EnemyController>();
    [SerializeField] string _dataJson;
    [SerializeField] float _timeReset;
    Coroutine _coroutineReset;
    void Start()
    {
        this.SpawnEnemyLocal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemyLocal()
    {
        this._localEnemy = EnemyManger.Instant.SpawmEnemy(_dataJson);

        for (int i = 0; i < _localEnemy.Count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * i * 0.7f;
            Vector3 newPosition = new Vector3(randomOffset.x, randomOffset.y, 0f) + this.transform.position;
            this._localEnemy[i].transform.position = newPosition;
            this._localEnemy[i].Init();

        }
        this._coroutineReset = StartCoroutine(processReset());
    }
   void ResetEnemyLocal()
    {
        for (int i = 0; i < _localEnemy.Count; i++)
        {
            if (_localEnemy[i].gameObject.activeSelf) continue;

            _localEnemy[i].setHp(_localEnemy[i].getDataEnemy()._HP);
            _localEnemy[i].setEnemyState(EnemyState.Run);
            _localEnemy[i].gameObject.SetActive(true);
        }
    }
   IEnumerator processReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeReset);
            if (_localEnemy.Count == 0) break;
            ResetEnemyLocal();
        }

    }

    private void OnDisable()
    {

        if (_coroutineReset != null)
            StopCoroutine(_coroutineReset);
    }
}
