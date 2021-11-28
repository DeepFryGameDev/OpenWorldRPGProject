using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandom : MonoBehaviour
{
    private List<float> lastNumbers;
    void Start()
    {
        lastNumbers = new List<float>();
        
        //Use a seed to get the same numbers every time.
        //https://docs.unity3d.com/ScriptReference/Random-state.html
        //Random.InitState(932903292);
    }

    public float GenerateRandomNumber(int min, int max)
    {
        Random.InitState((int)System.DateTime.Now.Ticks);

        //The highest number you want here. The lower number is inclusive, higher one exclusive.
        float rand = Random.Range(min, max);

        //Regenerate while the lastNumbers contains random.
        while (lastNumbers.Contains(rand))
        {
            rand = Random.Range(min, max);
        }

        //Store it.
        AddNumberToList(rand);

        //Give back the number we generated.
        return rand;
    }

    public float GenerateRandomNumber(float min, float max)
    {
        //The highest number you want here. The lower number is inclusive, higher one exclusive.
        float rand = Random.Range(min, max);

        //Regenerate while the lastNumbers contains random.
        while (lastNumbers.Contains(rand))
        {
            rand = Random.Range(min, max);
        }

        //Store it.
        AddNumberToList(rand);

        //Give back the number we generated.
        return rand;
    }

    void AddNumberToList(float number)
    {
        if (lastNumbers.Count > 3)
        {
            lastNumbers.RemoveAt(lastNumbers.Count - 1);
        }
        lastNumbers.Insert(0, number);
        //Debug.Log("Last numbers: ");
        for (int i = 0; i < lastNumbers.Count; i++)
        {
            //Debug.Log($"Index ({i}): {lastNumbers[i]}");
        }
    }
}