using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildingButton : MonoBehaviour
{
    [SerializeField]
    private GameObject buildingPrefab;

    [SerializeField]
    private Sprite sprite;
    
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int price;

    [SerializeField]
    private Text priceTxt;

    private Building buildingTemplate;

    public Building BuildingTemplate { get => buildingTemplate; }

    public GameObject BuildingPrefab
    {
        get
        {
            return buildingPrefab;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }
    
    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int Price => price;

    private void Start()
    {
        priceTxt.text = Price.ToString() + " $";

        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);

        GameObject building = Instantiate(BuildingPrefab);
        building.SetActive(false);
        buildingTemplate = building.transform.GetComponent<Building>();
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

    public virtual void OnBuildingPicked()
    {
        Hover.Instance.Activate(Icon, 0);
        GameManager.Instance.ToggleBuildPanel();
    }
}
