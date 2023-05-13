using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] 
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text priceTxt;
    
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }
    
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public int Price => price;

    private void Start()
    {
        priceTxt.text = Price.ToString() + " $";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }

    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.green;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
            priceTxt.color = Color.gray;
        }
    }

    public void ShowInfo()
    { 
        Tower tower = towerPrefab.GetComponentInChildren<Tower>();
        GameManager.Instance.SetTooltipText(tower.GetTooltipInfo());
        GameManager.Instance.ShowStats();
    }
}
