using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void CurrencyChanged();

public class GameManager : Singleton<GameManager>
{

    public event CurrencyChanged Changed;
    
    public BuildingButton ClickedBtn { get; set; }
    
    private int currency;

    private int lives;

    private bool gameOver = false;

    [SerializeField]
    private Text currencyTxt;

    [SerializeField]
    private Text livesTxt;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject levelFinishedMenu;
    
    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject buildPanel;

    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text statText;

    [SerializeField]
    private Text upgradePrice;

    [SerializeField]
    private GameObject inGameMenu;

    [SerializeField]
    private GameObject optionsMenu;

    private Tower selectedTower;

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
            this.currencyTxt.text = value.ToString() + "  <color=#FFD700>$</color>";
            
            OnCurrencyChanged();
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
        Currency = 30;
        string levelWavesName = "Level" + LevelStateController.level + "_waves";
        WaveManager.Instance.LoadLevel(levelWavesName); // TODO handle levels
    }

    void Update()
    {
        HandleEscape();
    }

    public void PickBuilding(BuildingButton buildingButton)
    {
        if (Currency >= buildingButton.Price && (!WaveManager.Instance.WaveActive || buildingButton.BuildingTemplate.CanBeBuiltDuringWave()))
        {
            this.ClickedBtn = buildingButton;
            buildingButton.OnBuildingPicked();
        }
    }

    public void BuyBuilding()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;

            if (Currency < ClickedBtn.Price)
            {
                Hover.Instance.Deactivate();
            }
        }
    }

    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.ToggleSelection();
        }
        
        selectedTower = tower;
        selectedTower.ToggleSelection();

        sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.ToggleSelection();
        }

        selectedTower = null;

        upgradePanel.SetActive(false);
    }

    public void DeselectSelection()
    {
        DeselectTower();
    }
    
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower == null && !Hover.Instance.IsVisible)
            {
                ShowInGameMenu();
            }
            else if (Hover.Instance.IsVisible)
            {
                DropTower();
            }
            else if (selectedTower != null)
            {
                DeselectTower();
            }
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
        SceneManager.LoadScene(0);
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

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetTooltipText(string txt)
    {
        statText.text = txt;
    }


    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTip();
    }

    public void UpdateUpgradeTip()
    {
        if (selectedTower != null)
        {
            sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";
            SetTooltipText(selectedTower.GetStats());
            
            if (selectedTower.NextUpgrade != null)
            {
                upgradePrice.text = selectedTower.NextUpgrade.Price.ToString() + " $";
            }
            else
            {
                upgradePrice.text = "Max Level";
            }
        }
    }

    public void UpgradeTower()
    {
        if (selectedTower != null && selectedTower.NextUpgrade != null && Currency >= selectedTower.NextUpgrade.Price)
        {
            selectedTower.Upgrade();
        }
    }

    public void ShowInGameMenu()
    {
        if (buildPanel.activeSelf)
        {
            ToggleBuildPanel();
        }

        if (optionsMenu.activeSelf)
        {
            ShowMain();
        }
        else
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    public void ShowOptions()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowMain()
    {
        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ToggleBuildPanel()
    {
        buildPanel.SetActive(!buildPanel.activeSelf);
    }

    public bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }

    public bool canShowNewWaveButton()
    {
        return !gameOver;
    }

    public void finishGame()
    {
        if (!gameOver)
        {
            gameOver = true;
            levelFinishedMenu.SetActive(true);
            if (LevelStateController.level == LevelStateController.maxLevel)
            {
                Image nextLevelButton = levelFinishedMenu.transform.GetChild(1).GetComponent<Image>();
                nextLevelButton.color = Color.gray;
            }
        }
    }

    public void nextLevel()
    {
        if (LevelStateController.level < LevelStateController.maxLevel)
        {
            LevelStateController.level += 1;
            SceneManager.LoadScene(2);
        }
    }
}
