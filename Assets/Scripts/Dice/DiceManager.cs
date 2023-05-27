using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : Singleton<DiceManager>
{
    [SerializeField]
    private int negativeThreshold = 3;

    [SerializeField]
    private int positiveThreshold = 3;

    [SerializeField]
    private float singleRollDurationSeconds;

    [SerializeField]
    private float totalRollDurationSeconds;

    [SerializeField]
    private float rollResultTiemoutSeconds;

    [SerializeField]
    private GameObject dicePanel;

    [SerializeField]
    private Image diceImage;

    [SerializeField]
    private Text diceStatus;

    [SerializeField]
    private Text randomEffectText;

    [SerializeField]
    private Button rollDiceButton;

    [SerializeField]
    private Sprite[] diceSides;

    private DiceEffectFactory diceEffectFactory;

    private DiceEffect currentEffect;

    public bool IsDicePanelShow { get => dicePanel.activeSelf; }

    public void Awake()
    {
        diceEffectFactory = new DiceEffectFactory(negativeThreshold, positiveThreshold);
    }

    public void Start()
    {
        WaveManager.Instance.onWaveFinished += new WaveFinished(handleFinishedWave);
    }

    public void RollDice()
    {
        GameManager.Instance.HideStats();
        deactivateButton();
        diceStatus.text = "Rolling!";
        randomEffectText.text = "";
        dicePanel.SetActive(true);

        StartCoroutine(runDiceAnimation());
    }

    public bool IsDisplayingEffects()
    {
        return dicePanel.activeSelf;
    }

    private void deactivateButton()
    {
        rollDiceButton.enabled = false;
        Image buttonImage = rollDiceButton.GetComponent<Image>();
        Text buttonText = rollDiceButton.GetComponentInChildren<Text>();

        buttonImage.color = new Color(1, 1, 1, 0.65f);
        buttonText.color = new Color(1, 1, 1, 0.5f);
    }

    private void activateButton()
    {
        rollDiceButton.enabled = true;
        Image buttonImage = rollDiceButton.GetComponent<Image>();
        Text buttonText = rollDiceButton.GetComponentInChildren<Text>();

        buttonImage.color = new Color(1, 1, 1, 1);
        buttonText.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator runDiceAnimation()
    {
        int previousSide = -1;
        int diceSide = 3;

        for(int i = 0; i < totalRollDurationSeconds / singleRollDurationSeconds; i++)
        {
            do {
                diceSide = Random.Range(1, diceSides.Length + 1);
            } while (diceSide == previousSide);

            previousSide = diceSide;
            diceImage.sprite = diceSides[diceSide - 1];
            
            yield return new WaitForSeconds(0.1f);
        }

        setDiceStatus(diceSide);

        currentEffect = diceEffectFactory.Generate(diceSide);
        randomEffectText.text = currentEffect.GetDescription();
        currentEffect.ApplyPreWave();

        yield return new WaitForSeconds(rollResultTiemoutSeconds);

        dicePanel.SetActive(false);
    }

    private void setDiceStatus(int result)
    {
        if (result < negativeThreshold)
        {
            diceStatus.text = "Maybe next time...";
        }
        else if (result > positiveThreshold)
        {
            diceStatus.text = "Lucky you!";
        }
        else
        {
            diceStatus.text = "Not bad!";
        }
    }

    private void handleFinishedWave()
    {
        if (currentEffect != null)
        {
            currentEffect.ApplyPostWave();
            currentEffect = null;
        }

        activateButton();
    }
}
