using System.Collections;
using UnityEngine;

public class CylinderPhysic : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;
    public float spawnInterval = 3.0f;
    public bool ispaused;
    public int count;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ispaused == false)
        {
            Time.timeScale = 0;
            ispaused = true;
            Debug.Log(ispaused);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ispaused == true)
        {
            Time.timeScale = 1;
            ispaused = false;
        }

        if (transform.position.x >= 15.50f)
        {
            Destroy(gameObject); // Sahnedeki objeyi yok et
            Debug.Log("Nesne yok edildi");
        }
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Yeni objeyi spawn et
            GameObject newObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            // Objeye rastgele bir renk ata
            AssignRandomColor(newObject);

            // Belirtilen süre kadar bekle
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void AssignRandomColor(GameObject obj)
    {
        // Objede Renderer bileþeni olup olmadýðýný kontrol et
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // Rastgele bir renk oluþtur
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            // Objeye bu rengi ata
            objRenderer.material.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Object does not have a Renderer component.");
        }
    }
}

