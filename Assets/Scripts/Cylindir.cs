using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindir : MonoBehaviour
{
    public Rigidbody2D cylindirRB;
    public float cylindirSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cylindirRB.AddForce(new Vector2(cylindirSpeed, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 11.50f)
        {
            Destroy(gameObject); // Sahnedeki objeyi yok et
            Debug.Log("Nesne yok edildi");
        }
    }
}
