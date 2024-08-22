using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindir : MonoBehaviour
{
    GameManager gameManager;
    public Rigidbody2D cylindirRB;
    public float cylindirSpeed;
    private AudioSource cylindirAudioSource;
    private Sound sfxSound;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cylindirRB.AddForce(new Vector2(cylindirSpeed, 0));

        // Yeni bir AudioSource bile�eni ekle ve sesi �nceden haz�rla
        cylindirAudioSource = gameObject.AddComponent<AudioSource>();

        // Rastgele bir ses se� ve ses kayna��n� haz�rla
        int randomIndex = UnityEngine.Random.Range(0, AudioManager.instance.sfxSounds.Length);
        sfxSound = AudioManager.instance.sfxSounds[randomIndex];
        cylindirAudioSource.clip = sfxSound.clip;
        cylindirAudioSource.playOnAwake = false;

        // Objenin b�y�kl���ne g�re ses seviyesini ayarla
        AdjustVolumeBasedOnSize();
    }

    private void Update()
    {
        if (gameObject.transform.position.x >= 14f)
        {
            Destroy(gameObject);
        }
    }

    private void AdjustVolumeBasedOnSize()
    {
        float sizeFactor = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;
        cylindirAudioSource.volume = Mathf.Clamp(1.0f / sizeFactor, 0.1f, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))
        {
            // Temas ba�lad���nda sesi an�nda �al
            cylindirAudioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))
        {
            // Temas bitti�inde sesi an�nda durdur
            cylindirAudioSource.Stop();
        }
    }
}
