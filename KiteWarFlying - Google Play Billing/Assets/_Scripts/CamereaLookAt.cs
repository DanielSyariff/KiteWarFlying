using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamereaLookAt : MonoBehaviour
{
    public Transform target; // Referensi ke transform objek yang ingin dilihat oleh kamera
    public float rotationSpeed = 5f; // Kecepatan rotasi kamera

    private void Update()
    {
        // Menghitung arah pandangan dari kamera ke objek target
        Vector3 direction = target.position - transform.position;

        // Menghitung rotasi yang menghadap ke arah objek target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Menginterpolasi rotasi kamera menuju rotasi yang diinginkan dengan kecepatan tertentu
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
