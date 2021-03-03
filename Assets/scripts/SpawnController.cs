using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject[] EnemyPrfabs;
    public GameObject[] BonusEnemyPrfabs;
    public Transform[] SpawnPoints;
    public float TimeBeforeSpawn;
    public int AmountOfEnemyes;
    public float TimeBeforeBonusSpawn;
    public int AmountOfBonusEnemyes;

    private EnemyController _enemyController;

    private Transform _playerTransform;
    private Transform _hammerCenterTransform;
    private RageController _rageController;
    private MainGameController _mainGameController;
    private void Start()
    {
        _mainGameController = FindObjectOfType<MainGameController>();
        _rageController = FindObjectOfType<RageController>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _hammerCenterTransform = GameObject.FindGameObjectWithTag("HammerCenter").GetComponent<Transform>();

        for (int i = 0; i < AmountOfEnemyes; i++)
        {
            Invoke("SpawnEnemy", TimeBeforeSpawn * i);
        }
    }
    private void SpawnEnemy()
    {
        _enemyController = Instantiate(EnemyPrfabs[Random.Range(0, EnemyPrfabs.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity).GetComponent<EnemyController>();
        _enemyController.SetParameters(_mainGameController, _playerTransform, _hammerCenterTransform, _rageController);
        if (_enemyController.IsBigEnemy)
        {
            _mainGameController.AddEnemyToCounter(3);
        }
        else
        {
            _mainGameController.AddEnemyToCounter(1);
        }
    }
    private void SpawnBonusEnemy()
    {
        _enemyController = Instantiate(BonusEnemyPrfabs[Random.Range(0, BonusEnemyPrfabs.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity).GetComponent<EnemyController>();
        _enemyController.SetParameters(_mainGameController, _playerTransform, _hammerCenterTransform, _rageController); if (_enemyController.IsBigEnemy)
        {
            _mainGameController.AddEnemyToCounter(3);
        }
        else
        {
            _mainGameController.AddEnemyToCounter(1);
        }
    }
    public void SpawnBonusEnemyes()
    {
        TimeBeforeSpawn = 0;
        for (int i = 0; i < AmountOfBonusEnemyes; i++)
        {
            Invoke("SpawnBonusEnemy", TimeBeforeBonusSpawn * i);
        }
    }
}
