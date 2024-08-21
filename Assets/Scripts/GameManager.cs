using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Prefab Ayarlarý")]
    public GameObject[] cylindirsPrefabs;
    public Transform spawnPoint;
    public int minSpawnsBeforeFirstObject = 3;
    public int maxSpawnsBeforeFirstObject = 6;

    [Header("Oynanýþ Ayarlarý")]
    public bool isPaused;
    public float minX;
    public float maxX;
    public float distanceToSpawnPoint;
    public float currentX;

    [Header("UI Elementleri")]
    public TextMeshProUGUI pausedText;
    public TextMeshProUGUI melodyText;

    private Animator animator;
    private string lastPausedText;
    private int melodyCount;
    private string melodyCountText;

    private float previousRearPivotX;
    private int spawnCountSinceLastFirstObject = 0;
    private int randomSpawnInterval;
    private GameObject newObject;
    private Transform childTransform;

    void Start()
    {
        animator = GetComponent<Animator>();
        randomSpawnInterval = Random.Range(minSpawnsBeforeFirstObject, maxSpawnsBeforeFirstObject);
        SpawnObjects();
    }

    void Update()
    {
        UpdateUI();
        HandleInput();
        HandleObjectSpawning();
        UpdateAnimator();
    }

    void UpdateUI()
    {
        melodyText.text = melodyCountText;
        pausedText.text = lastPausedText;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void HandleObjectSpawning()
    {
        if (!isPaused)
        {
            if (spawnPoint != null && childTransform != null)
            {
                currentX = childTransform.position.x;
                distanceToSpawnPoint = Vector3.Distance(spawnPoint.position, childTransform.position);

                if (spawnPoint.position.x <= currentX && distanceToSpawnPoint >= minX)
                {
                    SpawnObjects();
                }
            }
        }
    }

    void UpdateAnimator()
    {
        animator.Update(Time.unscaledDeltaTime);

        if (pausedText.text != lastPausedText)
        {
            animator.SetTrigger("PlayPausedText");
            lastPausedText = pausedText.text;
        }
    }

    void SpawnObjects()
    {
        if (!isPaused)
        {
            int prefabIndex = GetPrefabIndex();
            newObject = Instantiate(cylindirsPrefabs[prefabIndex], spawnPoint.position, spawnPoint.rotation);

            AssignRandomColor(newObject);

            if (prefabIndex == 1)
            {
                ScaleObject(newObject);
            }

            SetRearPivotTransform();

            if (prefabIndex == 0)
            {
                UpdateMelodyCount();
            }
        }
    }

    int GetPrefabIndex()
    {
        if (spawnCountSinceLastFirstObject >= randomSpawnInterval)
        {
            spawnCountSinceLastFirstObject = 0;
            randomSpawnInterval = Random.Range(minSpawnsBeforeFirstObject, maxSpawnsBeforeFirstObject);
            return 0;
        }
        else
        {
            spawnCountSinceLastFirstObject++;
            return Random.Range(1, cylindirsPrefabs.Length);
        }
    }

    void SetRearPivotTransform()
    {
        if (newObject != null)
        {
            childTransform = newObject.transform.Find("Rear Pivot");
            if (childTransform != null)
            {
                previousRearPivotX = childTransform.position.x;
            }
            else
            {
                Debug.LogWarning("Son oluþturulan objenin 'Rear Pivot' nesnesi bulunamadý.");
            }
        }
    }

    void UpdateMelodyCount()
    {
        melodyCount++;
        TextMeshProUGUI textMeshPro = newObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            melodyCountText = melodyCount.ToString();
            textMeshPro.text = $"{melodyCountText}. Melodi";
        }
        else
        {
            Debug.LogWarning("Son oluþturulan ilk objede TextMeshProUGUI bileþeni bulunamadý.");
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pausedText.text = "Baþlatmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 0f;
        Debug.Log("Oyun Duraklatýldý");
    }

    void ResumeGame()
    {
        isPaused = false;
        pausedText.text = "Durdurmak Ýçin \"Boþluk\" Tuþuna Bas.";
        Time.timeScale = 1f;
        Debug.Log("Oyun Devam Ediyor");
    }

    void AssignRandomColor(GameObject obj)
    {
        Transform slicedTransform = obj.transform.Find("Sliced");
        if (slicedTransform != null)
        {
            Renderer objRenderer = slicedTransform.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                Color currentColor = objRenderer.material.color;
                Color randomColor = RandomColor(currentColor);
                objRenderer.material.color = randomColor;
            }
        }
    }

    Color RandomColor(Color previousColor)
    {
        Color.RGBToHSV(previousColor, out float h, out float s, out float v);
        float randomH = (h + Random.Range(0.5f, 1.0f)) % 1.0f;
        float randomS = Random.Range(0.5f, 1.0f);
        float randomV = Random.Range(0.5f, 1.0f);
        return Color.HSVToRGB(randomH, randomS, randomV);
    }

    void ScaleObject(GameObject obj)
    {
        float scaleMultiplierX = Random.Range(minX, maxX);
        obj.transform.localScale = new Vector3(scaleMultiplierX, obj.transform.localScale.y, obj.transform.localScale.z);
    }
}
