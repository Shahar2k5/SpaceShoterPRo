using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _downSpeed = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _downSpeed);
        if (transform.position.y < -6f)
        {
            // respawn
            transform.position = new Vector3(Random.Range(-8, 8), 7.5f, 0);
        }
     }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayer();
            }

            Destroy(this.gameObject);
        }

        if(other.CompareTag("Laser"))
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        Debug.Log("HIT with " + other.name);
    }
}
