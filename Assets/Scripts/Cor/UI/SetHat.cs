using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHat : MonoBehaviour
{
    public GameObject[] listHat;
    public Transform hatsParent;
    void Start()
    {
        SetCurrentHat();
    }

    public void SetCurrentHat()
    {
        foreach (Transform t in hatsParent)
        {
            t.gameObject.SetActive(false);
        }
        int n = PlayerPrefs.GetInt("hat", 0);
        hatsParent.GetChild(n).gameObject.SetActive(true);
    }
}
