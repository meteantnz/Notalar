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
        _pauserText = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        lastPauserText = _pauserText;
        animator = GetComponent<Animator>();
        StartCoroutine(SpawnObjects());
    }

    void Update()
    {
        pausedText.text = _pauserText;

        // Time.timeScale = 0 olsa bile animasyonun çalýþmasý için Animator'u manuel olarak güncelle
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

        // Deðer deðiþikliði olup olmadýðýný kontrol et ve animasyonu tetikle
        if (_pauserText != lastPauserText)
        {
            animator.SetTrigger("PlayPausedText"); // Tetikleyiciyi ayarla
            lastPauserText = _pauserText; // Son metni güncelle
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (!isPaused) // Sadece oyun duraklatýlmadýðýnda spawn iþlemini yap
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
        _pauserText = "Baþlatmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 0f; // Oyun durduruluyor
        Debug.Log("Oyun Duraklatýldý");
    }

    void ResumeGame()
    {
        isPaused = false;
        _pauserText = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 1f; // Oyun tekrar baþlatýlýyor
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
