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
    private GameObject _tripleShotLaserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    
    [SerializeField]
    private float _canFire = -1f;

    [SerializeField]
    private int _playerLife = 3;

    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private bool isTripleShotAvailable = false;
    private bool _isShieldOn = false;

    private void Start()
    {
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
            Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
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
}
