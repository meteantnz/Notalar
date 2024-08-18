using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] cylindirsPrefabs;
    public Transform spawnPoint;
    public float spawnInterval = 3.0f;
    public bool isPaused;
    public TextMeshProUGUI pausedText;
    public float sizeVariation = 0.5f; // Boyut varyasyonu oraný
    private Animator animator;
    private string lastPauserText;
    public float rearPivotDistance;
    public float rearPivotDistanceNegative;
    public float frontPivotDistance;
    public float pivotDistance;
    public float pivotDistance2;
    public float minX;
    public float maxX;
    private GameObject newObject;
    private GameObject randomPrefab;

    private float previousRearPivotX; // Önceki X konumu

    void Start()
    {
        SpawnObjects();
        lastPauserText = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        animator = GetComponent<Animator>();
        if (newObject != null)
        {
            Transform childTransform = newObject.transform.Find("Rear Pivot");
            if (childTransform != null)
            {
                previousRearPivotX = childTransform.position.x; // Ýlk X konumunu kaydet
            }
        }
    }

    void Update()
    {
        pausedText.text = lastPauserText;

        if (newObject != null)
        {
            // Çocuða eriþim (çocuðun adý "Rear Pivot" olarak varsayýlmýþtýr)
            Transform childTransform = newObject.transform.Find("Rear Pivot");

            if (childTransform != null)
            {
                // Çocuðun pozisyonunu al
                Vector3 childPosition = childTransform.position;
                // Mesafeyi hesapla
                rearPivotDistance = Vector3.Distance(spawnPoint.position, childPosition);

                // Mesafeyi güncelle
                float currentRearPivotX = childPosition.x;
                rearPivotDistanceNegative = currentRearPivotX - previousRearPivotX; // X eksenindeki deðiþimi hesapla
                previousRearPivotX = currentRearPivotX; // Önceki X konumunu güncelle

                // Çocuðun pozisyonundan mesafe hesapla
                frontPivotDistance = Vector3.Distance(spawnPoint.position, newObject.transform.position);
            }
            else
            {
                Debug.LogWarning("Çocuk objesi bulunamadý.");
            }
        }

        if (rearPivotDistance >= pivotDistance && frontPivotDistance >= pivotDistance2)
        {
            SpawnObjects();
        }

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
        if (pausedText.text != lastPauserText)
        {
            animator.SetTrigger("PlayPausedText"); // Tetikleyiciyi ayarla
            lastPauserText = pausedText.text; // Son metni güncelle
        }
    }

    void SpawnObjects()
    {
        if (!isPaused) // Sadece oyun duraklatýlmadýðýnda spawn iþlemini yap
        {
            randomPrefab = cylindirsPrefabs[Random.Range(0, cylindirsPrefabs.Length)];
            newObject = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);
            AssignRandomColor(newObject);
            ScaleObject(newObject);
            // Çocuða eriþim ve mesafe hesaplama
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pausedText.text = "Baþlatmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 0f; // Oyun durduruluyor
        Debug.Log("Oyun Duraklatýldý");
    }

    void ResumeGame()
    {
        isPaused = false;
        pausedText.text = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 1f; // Oyun tekrar baþlatýlýyor
        Debug.Log("Oyun Devam Ediyor");
    }

    void AssignRandomColor(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // Önceki rengin zýt veya zýtta yakýn bir rengini seç
            Color currentColor = objRenderer.material.color;
            Color randomColor = RandomColor(currentColor);
            objRenderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Object does not have a Renderer component.");
        }
    }

    Color RandomColor(Color previousColor)
    {
        float h, s, v;
        Color.RGBToHSV(previousColor, out h, out s, out v);
        float randomH = (h + Random.Range(0.5f, 1.0f)) % 1.0f;
        float randomS = Random.Range(0.5f, 1.0f);
        float randomV = Random.Range(0.5f, 1.0f);
        return Color.HSVToRGB(randomH, randomS, randomV);
    }

    void ScaleObject(GameObject obj)
    {
        // Rastgele bir x ekseni ölçek oraný belirle (minX ile maxX arasýnda)
        float scaleMultiplierX = Random.Range(minX, maxX);
        // Diðer eksenlerdeki ölçekleri deðiþtirmeden yalnýzca x eksenini ölçeklendir
        obj.transform.localScale = new Vector3(scaleMultiplierX, obj.transform.localScale.y, obj.transform.localScale.z);
    }
}
