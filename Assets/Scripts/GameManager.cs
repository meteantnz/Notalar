using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    Cylindir Cylindir;
    public GameObject prefab;
    public Transform spawnPoint;
    public float spawnInterval = 3.0f;
    public bool ispaused;
    public int count;
    public TextMeshProUGUI pausedText;
    public string _pauserText;
    private Animator animator;
    private string lastPauserText;

    void Start()
    {
        Cylindir=FindObjectOfType<Cylindir>();
        _pauserText = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        lastPauserText = _pauserText;
        animator = GetComponent<Animator>();
        StartCoroutine(SpawnObjects());
    }

    private void Update()
    {
        pausedText.text = _pauserText;

        // Time.timeScale = 0 olsa bile animasyonun çalýþmasý için Animator'u manuel olarak güncelle
        animator.Update(Time.unscaledDeltaTime);

        // Deðer deðiþikliði olup olmadýðýný kontrol et
        if (_pauserText != lastPauserText)
        {
            animator.SetTrigger("PlayPausedText");
            lastPauserText = _pauserText; // Son metni güncelle
        }

        if (Input.GetKeyDown(KeyCode.Space) && !ispaused)
        {
            ispaused = true;
            animator.SetTrigger(_pauserText);
            _pauserText = "Baþlatmak Ýçin \"Boþluk\" Tuþuna Bas.";

            Debug.Log(ispaused);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ispaused)
        {           
            ispaused = false;
            _pauserText = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        }
    }

    IEnumerator SpawnObjects()
    {
        if (ispaused == false)
        {
            while (true)
            {
                GameObject newObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                AssignRandomColor(newObject);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
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
