using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    
    public TowerButton ClickedBtn { get; set; }
    
    private int currency;

    private int wave = 0;

    private int lives;

    private bool gameOver = false;

    private int health = 15;

    [SerializeField]
    private Text waveTxt;

    [SerializeField]
    private Text currencyTxt;

    [SerializeField]
    private Text livesTxt;

    [SerializeField]
    private GameObject waveBtn;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private Text sellText;

    private Tower selectedTower;

    List<EnemyShip> activeEnemies = new List<EnemyShip>();
    
    public ObjectPool Pool { get; set; }

    public bool WaveActive { 
        get 
        { 
            return activeEnemies.Count > 0; 
        } 
    }

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

    public int Lives
    {
        get
        {
            return lives;
        }

        set
        {
            this.lives = value;

            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }

            this.livesTxt.text = lives.ToString();
        }
    }


    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    
    void Start()
    {
        Lives = 3;
        Currency = 120;
    }

    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerButton towerButton)
    {
        if (Currency >= towerButton.Price && !WaveActive)
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

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        
        selectedTower = tower;
        selectedTower.Select();

        sellText.text = "+ " + (selectedTower.Price / 2);

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = null;

        upgradePanel.SetActive(false);
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
        wave++;
        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        for (int i = 0; i < wave; i++)
        {
            int enemyIndex = Random.Range(0, 1);

            string type = string.Empty;

            switch (enemyIndex)
            {
                case 0:
                    type = "FirstEnemy";
                    break;
            }

            EnemyShip enemy = Pool.GetObject(type).GetComponent<EnemyShip>();
            enemy.Spawn(health);

            if (wave % 3 == 0)
            {
                health += 5;
            }

            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(2.5f);
        }
        
    }

    public void RemoveEnemy(EnemyShip enemy)
    {
        activeEnemies.Remove(enemy);
        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;

            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            Destroy(selectedTower.transform.parent.gameObject);

            DeselectTower();
        }
    }
}
