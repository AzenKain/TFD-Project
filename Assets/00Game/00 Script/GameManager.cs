using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Idle,
    Pause,
    Play,
    Over
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController _player;
    public GameState _gameState { get; private set; } = GameState.Play;
    public PlayerController player => _player;

    [SerializeField] List<GoldBase> _goldPrefab = new List<GoldBase>();
    public List<GoldBase> goldPrefab => _goldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawmGold(int value, Vector2 position)
    {
        for (int i = 0; i < _goldPrefab.Count; i++)
        {
            int valueInit = value / _goldPrefab[i].getValueGold();
            SingleSpawm(valueInit, position, _goldPrefab[i]);
            value %= _goldPrefab[i].getValueGold();
        }
    }
    void SingleSpawm(int valueInit, Vector2 position, GoldBase golds)
    {
        for(int i = 0; i < valueInit; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector2 spawnPosition = position + randomOffset;
            GoldBase goldInstant = ObjectPooling.Instant.getComp<GoldBase>(golds);
            goldInstant.transform.position = spawnPosition;
            goldInstant.gameObject.SetActive(true);
        }
    }
    public void UpdateGameState(GameState gS)
    {
        this._gameState = gS;
    }
}
