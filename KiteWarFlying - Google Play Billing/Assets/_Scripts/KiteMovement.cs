using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KiteMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Kecepatan pergerakan objek
    public float smoothRotation = 5f;  // Kecepatan penghalusan rotasi

    private Vector2 movement;  // Vektor pergerakan objek

    public Transform enemyParent;
    public Transform layanganParent;
    public Transform layangan;
    public SpriteRenderer spriteRenderer;
    public GameObject rope;
    public bool enemyFound;
    public bool isAlive = true;

    public QTEBattle qteBattle;

    public bool useJoystcik;
    public MovementJoystick movementJoystick;

    public List<SpriteData> kitesprite;

    private void Start()
    {
        spriteRenderer.sprite = kitesprite[PlayerPrefs.GetInt("SelectedLayangan")].sprite;
    }

    private void Update()
    {
        if (!enemyFound)
        {
            if (isAlive)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");

                // Hitung pergerakan objek berdasarkan input dan kecepatan
                if (useJoystcik)
                {
                    movement = new Vector2(movementJoystick.joystickVec.x, movementJoystick.joystickVec.y) * moveSpeed;
                }
                else
                {
                    movement = new Vector2(horizontalInput, verticalInput) * moveSpeed;
                }

                // Mengatur rotasi objek berdasarkan arah pergerakan dengan penghalusan
                if (movement.magnitude > 0f)
                {
                    float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    layangan.rotation = Quaternion.Slerp(layangan.rotation, targetRotation, smoothRotation * Time.deltaTime);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!enemyFound)
        {
            if (isAlive)
            {
                // Memperbarui posisi objek menggunakan pergerakan dan waktu tetap
                Vector3 newPosition = transform.position + new Vector3(movement.x, movement.y, 0f) * Time.fixedDeltaTime;
                transform.position = newPosition;
            }
        }
    }

    public void PlayerDeath()
    {
        isAlive = false;
        enemyFound = false;
        layangan.GetComponent<Rigidbody>().useGravity = true;
        rope.SetActive(false);

        ScoreAndResultManager.instance.ShowScore(false);
    }
}
