using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour
{
    public GameObject targetObject;  // Objek 3D yang akan diubah posisi Z-nya
    public Slider slider;  // UI Slider untuk mengubah posisi Z
    public float transitionTime = 1f;  // Waktu transisi dalam detik

    private float targetPositionZ;  // Nilai posisi Z target
    private float initialPositionZ;  // Nilai posisi Z awal
    private float transitionTimer;  // Timer untuk melacak waktu transisi

    public TextMeshProUGUI distanceMeterText;

    private void Start()
    {
        slider.navigation = new Navigation { mode = Navigation.Mode.None };

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Inisialisasi nilai posisi awal dan target
        initialPositionZ = targetObject.transform.position.z;
        targetPositionZ = initialPositionZ;
    }

    private void Update()
    {
        // Menghitung progress transisi
        float progress = Mathf.Clamp01(transitionTimer / transitionTime);

        // Melakukan transisi posisi Z
        float currentPositionZ = Mathf.Lerp(initialPositionZ, targetPositionZ, progress);

        distanceMeterText.text = (currentPositionZ).ToString("0") + "m";

        // Mengupdate posisi Z objek
        Vector3 newPosition = targetObject.transform.position;
        newPosition.z = currentPositionZ;
        targetObject.transform.position = newPosition;

        // Mengupdate timer transisi
        transitionTimer += Time.deltaTime;
    }

    private void OnSliderValueChanged(float value)
    {
        // Mengatur target posisi Z berdasarkan nilai slider
        initialPositionZ = targetObject.transform.position.z;
        targetPositionZ = value;

        // Reset timer transisi
        transitionTimer = 0f;
    }
}
