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
    public float sizeVariation = 0.5f; // Boyut varyasyonu oran�
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

    private float previousRearPivotX; // �nceki X konumu

    void Start()
    {
        SpawnObjects();
        lastPauserText = "Durdurmak ��in \"Bo�luk\" Tu�una Bas.";
        animator = GetComponent<Animator>();
        if (newObject != null)
        {
            Transform childTransform = newObject.transform.Find("Rear Pivot");
            if (childTransform != null)
            {
                previousRearPivotX = childTransform.position.x; // �lk X konumunu kaydet
            }
        }
    }

    void Update()
    {
        pausedText.text = lastPauserText;

        if (newObject != null)
        {
            // �ocu�a eri�im (�ocu�un ad� "Rear Pivot" olarak varsay�lm��t�r)
            Transform childTransform = newObject.transform.Find("Rear Pivot");

            if (childTransform != null)
            {
                // �ocu�un pozisyonunu al
                Vector3 childPosition = childTransform.position;
                // Mesafeyi hesapla
                rearPivotDistance = Vector3.Distance(spawnPoint.position, childPosition);

                // Mesafeyi g�ncelle
                float currentRearPivotX = childPosition.x;
                rearPivotDistanceNegative = currentRearPivotX - previousRearPivotX; // X eksenindeki de�i�imi hesapla
                previousRearPivotX = currentRearPivotX; // �nceki X konumunu g�ncelle

                // �ocu�un pozisyonundan mesafe hesapla
                frontPivotDistance = Vector3.Distance(spawnPoint.position, newObject.transform.position);
            }
            else
            {
                Debug.LogWarning("�ocuk objesi bulunamad�.");
            }
        }

        if (rearPivotDistance >= pivotDistance && frontPivotDistance >= pivotDistance2)
        {
            SpawnObjects();
        }

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
        if (pausedText.text != lastPauserText)
        {
            animator.SetTrigger("PlayPausedText"); // Tetikleyiciyi ayarla
            lastPauserText = pausedText.text; // Son metni g�ncelle
        }
    }

    void SpawnObjects()
    {
        if (!isPaused) // Sadece oyun duraklat�lmad���nda spawn i�lemini yap
        {
            randomPrefab = cylindirsPrefabs[Random.Range(0, cylindirsPrefabs.Length)];
            newObject = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);
            AssignRandomColor(newObject);
            ScaleObject(newObject);
            // �ocu�a eri�im ve mesafe hesaplama
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pausedText.text = "Ba�latmak ��in \"Bo�luk\" Tu�una Bas.";
        Time.timeScale = 0f; // Oyun durduruluyor
        Debug.Log("Oyun Duraklat�ld�");
    }

    void ResumeGame()
    {
        isPaused = false;
        pausedText.text = "Durdurmak ��in \"Bo�luk\" Tu�una Bas.";
        Time.timeScale = 1f; // Oyun tekrar ba�lat�l�yor
        Debug.Log("Oyun Devam Ediyor");
    }

    void AssignRandomColor(GameObject obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // �nceki rengin z�t veya z�tta yak�n bir rengini se�
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
        // Rastgele bir x ekseni �l�ek oran� belirle (minX ile maxX aras�nda)
        float scaleMultiplierX = Random.Range(minX, maxX);
        // Di�er eksenlerdeki �l�ekleri de�i�tirmeden yaln�zca x eksenini �l�eklendir
        obj.transform.localScale = new Vector3(scaleMultiplierX, obj.transform.localScale.y, obj.transform.localScale.z);
    }
}
