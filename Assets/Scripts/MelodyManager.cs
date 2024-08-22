using System.Collections.Generic;
using UnityEngine;

public class MelodyManager : MonoBehaviour
{
    public static MelodyManager instance;
    public List<float> currentMelody;
    public float melodyLength = 8f;
    public float[] possibleValues = { 0.25f, 0.5f, 1f, 1.5f, 2f };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateMelody()
    {
        currentMelody = new List<float>();
        float totalLength = 0f;

        while (totalLength < melodyLength)
        {
            float value = possibleValues[Random.Range(0, possibleValues.Length)];
            if (totalLength + value > melodyLength)
            {
                value = melodyLength - totalLength;
            }
            currentMelody.Add(value);
            totalLength += value;
        }
    }
}
