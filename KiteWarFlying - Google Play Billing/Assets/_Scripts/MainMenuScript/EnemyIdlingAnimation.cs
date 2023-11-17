using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdlingAnimation : MonoBehaviour
{
    public Rigidbody layangan;
    public GameObject rope;
    public SpriteRenderer sprite;
    public Sprite[] spriteList;
    public Material[] ropeColorList;

    public float rotationSpeed = 10f;  // Kecepatan rotasi objek
    public float targetAngle = 25f;  // Sudut target rotasi

    private Quaternion initialRotation;  // Rotasi awal objek
    private float currentAngle;  // Sudut rotasi saat ini

    public float moveSpeed = 5f;  // Kecepatan pergerakan objek
    public float resetDistance = 10f;  // Jarak maksimum dari posisi awal sebelum kembali

    private Vector3 initialPosition;  // Posisi awal objek
    private Vector3 targetPosition;  // Posisi target acak
    // Start is called before the first frame update
    void Start()
    {
        int tmpKiteSprite = Random.Range(0, spriteList.Length);
        sprite.sprite = spriteList[tmpKiteSprite];

        int tmpRopeColor = Random.Range(0, ropeColorList.Length);
        rope.GetComponent<MeshRenderer>().material = ropeColorList[tmpRopeColor];

        initialRotation = transform.rotation;
        currentAngle = 0f;

        initialPosition = transform.position;
        GenerateTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // Menghitung sudut rotasi saat ini
        currentAngle += rotationSpeed * Time.deltaTime;

        // Mengatur rotasi objek berdasarkan sudut rotasi saat ini
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, currentAngle);
        transform.rotation = targetRotation;

        // Mengubah arah rotasi saat mencapai sudut target positif atau negatif
        if (currentAngle >= targetAngle || currentAngle <= -targetAngle)
        {
            rotationSpeed *= -1f;
        }


        // Memperbarui posisi objek menuju posisi target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Memeriksa jika objek mencapai posisi target
        if (transform.position == targetPosition)
        {
            GenerateTargetPosition();
        }

        // Memeriksa jika objek mencapai jarak maksimum dari posisi awal
        if (Vector3.Distance(transform.position, initialPosition) >= resetDistance)
        {
            ResetPosition();
        }
    }

    private void GenerateTargetPosition()
    {
        // Menciptakan posisi target acak dalam radius resetDistance
        targetPosition = initialPosition + Random.insideUnitSphere * resetDistance;
    }

    private void ResetPosition()
    {
        // Mengatur posisi objek kembali ke posisi awal
        transform.position = initialPosition;
        GenerateTargetPosition();
    }
}
