using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    private bool _playerAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void PlayerDied()
    {
        _playerAlive = false;       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_playerAlive)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
            newEnemy.transform.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(2);
        }
    }
}
