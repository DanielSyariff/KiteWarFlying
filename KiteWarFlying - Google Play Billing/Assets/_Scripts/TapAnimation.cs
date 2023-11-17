using UnityEngine;
using DG.Tweening;

public class TapAnimation : MonoBehaviour
{
    public float scaleDuration = 0.2f;
    public float scaleAmount = 1.2f;

    private void Start()
    {
        // Mengulang animasi secara terus-menerus
        PlayTapAnimation();
        //Invoke("PlayTapAnimation", 0f);
    }

    private void PlayTapAnimation()
    {
        // Mengatur skala awal objek
        transform.localScale = Vector3.one;

        // Menentukan sequence animasi dengan DoTween
        Sequence sequence = DOTween.Sequence();

        // Animasi skala objek menjadi lebih besar
        sequence.Append(transform.DOScale(scaleAmount, scaleDuration));

        // Animasi skala objek kembali ke ukuran semula
        sequence.Append(transform.DOScale(1f, scaleDuration));

        // Menentukan delay antara animasi tap
        sequence.AppendInterval(0.2f);

        // Mengulang sequence animasi
        sequence.OnComplete(() => PlayTapAnimation());

        // Memulai sequence animasi
        sequence.Play();
    }
}
