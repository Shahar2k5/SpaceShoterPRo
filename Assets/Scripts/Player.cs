using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleLaser;

    private UIUpdater _uiUpdater;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisualizer;

    private bool isTripleShotAvailable = false;
    [SerializeField]
    private bool _isShieldOn = false;
    [SerializeField]
    private int _playerLife = 3;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;

    [SerializeField]
    private int _score = 0;

    private void Start()
    {
        _uiUpdater = GameObject.Find("Canvas").GetComponent<UIUpdater>();
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        _shieldVisualizer.SetActive(false);
    }

    public void DamagePlayer()
    {
        if (_isShieldOn)
        {
            _isShieldOn = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _playerLife--;
        if (_playerLife == 0)
        {
            if (_spawnManager != null)
            {
                _spawnManager.PlayerDied();
            }
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (isTripleShotAvailable)
        {
            Instantiate(_tripleLaser, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 6), 0);

        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }

    public void enabledTripleShot()
    {
        isTripleShotAvailable = true;
        StartCoroutine(TripleShotRoutine());
    }

    public void enableSuperSpeed()
    {
        _speed= 15;
        StartCoroutine(SuperSpeedRoutine());
    }

    public void enableShield()
    {
        _shieldVisualizer.SetActive(true);
        _isShieldOn = true;
    }

    IEnumerator TripleShotRoutine()
    {
        yield return new WaitForSeconds(5);
        isTripleShotAvailable = false;
    }

    IEnumerator SuperSpeedRoutine()
    {
        yield return new WaitForSeconds(5);
        _speed = 10;
    }

    public void EnemyKilled()
    {
        _score += 10;
        _uiUpdater.UpdateScore(_score);
    }
}
