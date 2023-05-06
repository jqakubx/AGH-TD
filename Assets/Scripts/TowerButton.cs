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
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.gray;
            priceTxt.color = Color.gray;
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;

        // TODO: dodać opisy jak będą różne wieże i ich debufy
        //                 Tower tower = towerPrefab.GetComponentInChildren<PodtypTower>();

        switch (type)
        {
            case "First":
                Tower tower = towerPrefab.GetComponentInChildren<Tower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>First Tower</b></size></color>" +
                                        "\nDamage: {0}", tower.Damage);
                break;
            case "Second":
                Tower tower2 = towerPrefab.GetComponentInChildren<Tower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Second Tower</b></size></color>" +
                                        "\nDamage: {0}" +
                                        "\nA little stronger", tower2.Damage);              
                break;
            case "Catapult":
                Tower catapult = towerPrefab.GetComponentInChildren<Tower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Catapult</b></size></color>" +
                                        "\nDamage: {0}" +
                                        "\nA little stronger", catapult.Damage);              
                break;
        }
        
        GameManager.Instance.SetTooltipText(tooltip);
        GameManager.Instance.ShowStats();
    }
}
