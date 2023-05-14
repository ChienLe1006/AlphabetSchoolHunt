using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetHolder : MonoBehaviour
{
    [SerializeField] private float distanceBetweenAlphabet = 0.2f;
    internal void Rerange()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localPosition = new Vector3(0, i * distanceBetweenAlphabet, 0);
        }
    }
}
