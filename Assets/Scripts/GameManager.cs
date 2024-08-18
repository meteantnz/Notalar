using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Cylindir Cylindir;
    public GameObject prefab;
    public Transform spawnPoint;
    public float spawnInterval = 3.0f;
    public bool isPaused;
    public int count;
    public TextMeshProUGUI pausedText;
    public string _pauserText;
    private Animator animator;
    private string lastPauserText;

    void Start()
    {
        Cylindir = FindObjectOfType<Cylindir>();
        _pauserText = "Durdurmak ��in \"Bo�luk\" Tu�una Bas.";
        lastPauserText = _pauserText;
        animator = GetComponent<Animator>();
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        pausedText.text = _pauserText;

        // Time.timeScale = 0 olsa bile animasyonun �al��mas� i�in Animator'u manuel olarak g�ncelle
        animator.Update(Time.unscaledDeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        // De�er de�i�ikli�i olup olmad���n� kontrol et ve animasyonu tetikle
        if (_pauserText != lastPauserText)
        {
            animator.SetTrigger("PlayPausedText"); // Tetikleyiciyi ayarla
            lastPauserText = _pauserText; // Son metni g�ncelle
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (!isPaused) // Sadece oyun duraklat�lmad���nda spawn i�lemini yap
            {
                GameObject newObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                AssignRandomColor(newObject);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void PauseGame()
    {
        isPaused = true;
        _pauserText = "Ba�latmak ��in \"Bo�luk\" Tu�una Bas.";
        Time.timeScale = 0f; // Oyun durduruluyor
        Debug.Log("Oyun Duraklat�ld�");
    }

    void ResumeGame()
    {
        isPaused = false;
        _pauserText = "Durdurmak ��in \"Bo�luk\" Tu�una Bas.";
        Time.timeScale = 1f; // Oyun tekrar ba�lat�l�yor
        Debug.Log("Oyun Devam Ediyor");
    }

    void AssignRandomColor(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            objRenderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Object does not have a Renderer component.");
        }
    }
}
