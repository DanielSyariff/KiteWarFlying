using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteRopeDetection : MonoBehaviour
{
    public KiteMovement kiteMovement;

    private void Start()
    {
        kiteMovement = FindObjectOfType<KiteMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // Periksa apakah objek yang bersentuhan memiliki tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Objek dengan tag "Enemy" telah terdeteksi
            Debug.Log("Trigger dengan objek musuh terdeteksi!");
            kiteMovement.enemyFound = true;
            kiteMovement.qteBattle.StartQTE(other.GetComponentInParent<EnemyKite>());
        }

        if (other.CompareTag("Ground"))
        {
            kiteMovement.PlayerDeath();
        }
    }
}
