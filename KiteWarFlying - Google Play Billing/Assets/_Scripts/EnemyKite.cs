using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyKite : MonoBehaviour
{
    public BGMController bgmController;
    public float reductionPower;
    private int KillScore;
    private int CoinScore;
    public Rigidbody layangan;
    public GameObject rope;
    public SpriteRenderer sprite;
    public Sprite[] spriteList;
    public Material[] ropeColorList;
    public GameObject prefabToSpawn;

    public KiteMovement kiteMovement;

    public float rotationSpeed = 10f;  // Kecepatan rotasi objek
    public float targetAngle = 25f;  // Sudut target rotasi

    private Quaternion initialRotation;  // Rotasi awal objek
    private float currentAngle;  // Sudut rotasi saat ini

    public float moveSpeed = 5f;  // Kecepatan pergerakan objek
    public float resetDistance = 10f;  // Jarak maksimum dari posisi awal sebelum kembali

    private Vector3 initialPosition;  // Posisi awal objek
    private Vector3 targetPosition;  // Posisi target acak

    private bool isDeath;

    public bool fromRespawn;

    private void Start()
    {
        bgmController = FindObjectOfType<BGMController>();
        reductionPower = Random.Range(0.5f, 2.5f);
        if (reductionPower < 1)
        {
            KillScore = 10;
            CoinScore = 25;
        }
        else
        {
            KillScore = 25;
            CoinScore = 50;
        }

        int tmpKiteSprite = Random.Range(0, spriteList.Length);
        sprite.sprite = spriteList[tmpKiteSprite];

        int tmpRopeColor = Random.Range(0, ropeColorList.Length);
        rope.GetComponent<MeshRenderer>().material = ropeColorList[tmpRopeColor];

        kiteMovement = FindObjectOfType<KiteMovement>();
        initialRotation = transform.rotation;
        currentAngle = 0f;

        initialPosition = transform.position;
        GenerateTargetPosition();

        if (fromRespawn)
        {
            this.transform.localScale = Vector3.zero;
            StartCoroutine(DoScaleSpawn());
        }
    }

    IEnumerator DoScaleSpawn()
    {
        yield return new WaitForSeconds(5f);
        this.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
    }

    private void Update()
    {
        if (!isDeath)
        {
            if (!kiteMovement.enemyFound)
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

    public void Death()
    {
        bgmController.PlaySFX("SFXKiteDown");
        GameObject nextLayangan = Instantiate(prefabToSpawn, kiteMovement.enemyParent);
        nextLayangan.GetComponent<EnemyKite>().fromRespawn = true;
        nextLayangan.transform.position = transform.position;

        rope.SetActive(false);
        layangan.useGravity = true;
        isDeath = true;
        kiteMovement.enemyFound = false;

        ScoreAndResultManager.instance.IncreaseScoreAndCheckKill(KillScore, CoinScore); ;
    }
}
