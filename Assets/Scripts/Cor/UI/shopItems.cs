using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopItems : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isBuy = false;
    void Start()
    {
        int n = PlayerPrefs.GetInt(transform.name, 0);
        if (n == 1||isBuy)
        {
            isBuy = true;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }
            if ( PlayerPrefs.GetInt("hatselect", 0)==transform.GetSiblingIndex())
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }else
            {
                transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        
        else
        {
            ShopControl.Instance.hats.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect()
    {
        if(isBuy)
        {
            shopItems[] s = transform.parent.GetComponentsInChildren<shopItems>();
            foreach (shopItems shop in s)
            {
                shop.transform.GetChild(2).gameObject.SetActive(false);
            }
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }
            PlayerPrefs.SetInt("hat", transform.GetSiblingIndex());
            ShopControl.Instance.SetHats();
            PlayerPrefs.SetInt("hatselect", transform.GetSiblingIndex());
        }
    }

    public void Buy()
    {
        isBuy = true;
        PlayerPrefs.SetInt(transform.name, 1);
        shopItems[] s = transform.parent.GetComponentsInChildren<shopItems>();
        foreach (shopItems shop in s)
        {
            shop.transform.GetChild(2).gameObject.SetActive(false);
        }
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
        PlayerPrefs.SetInt("hat", transform.GetSiblingIndex());
        ShopControl.Instance.SetHats();
        PlayerPrefs.SetInt("hat", transform.GetSiblingIndex());
        ShopControl.Instance.hats.Remove(this);
    }
}
