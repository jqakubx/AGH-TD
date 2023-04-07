using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    
    public TowerButton ClickedBtn { get; set; }
    
    private int currency;

    [SerializeField]
    private Text currencyTxt;
    
    public ObjectPool Pool { get; set; }

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString() + "  <color=lime>$</color>";
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    
    void Start()
    {
        Currency = 120;
    }

    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerButton towerButton)
    {
        if (Currency >= towerButton.Price)
        {
            this.ClickedBtn = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();
        
        int enemyIndex = Random.Range(0, 1);

        string type = string.Empty;

        switch (enemyIndex)
        {
            case 0:
                type = "FirstEnemy";
                break;
        }

        EnemyShip enemy = Pool.GetObject(type).GetComponent<EnemyShip>();
        enemy.Spawn();
        
        yield return new WaitForSeconds(2.5f);
    }
}
