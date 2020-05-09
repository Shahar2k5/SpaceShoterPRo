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
    private float _fireRate = 0.5f;

    [SerializeField]
    private float _canFire = -1f;

    [SerializeField]
    private int _playerLife = 3;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        Debug.Log("this is " + _spawnManager);
    }

    public void DamagePlayer()
    {
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

    // Update is called once per frame
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
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.7f,0), Quaternion.identity);
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
}
