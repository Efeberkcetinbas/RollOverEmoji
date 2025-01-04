using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[Serializable]
public class HelperProperties
{
    public Sprite PurchaseIcon;
    public string PurchaseHeader;
    public string PurchaseBrief;


    public Button BuyButton;
    public Button UseButton;
    public GameObject Locked;
    public TextMeshProUGUI UseAmountText;
    public TextMeshProUGUI UnlockedLevelText;
    public HelperConfig helperConfig;
}

public class HelperManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private List<HelperProperties> helperProperties=new List<HelperProperties>();

    [Header("Helper Particles")]
    [SerializeField] private ParticleSystem rotatesParticle;
    [SerializeField] private ParticleSystem cleanerParticle;
    [SerializeField] private ParticleSystem collect3Particle;

    [Header("Purchase Panel")]
    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private TextMeshProUGUI headerText,purchaseAmountText,briefText,purchasePriceText;
    [SerializeField] private Image purchaseIcon;
    [SerializeField] private Button purchaseBuyButton;
    

    private bool isUsingHelper=false;
    private int buyIndex=0;

    private void Start()
    {
        AssignHelperImages();
        CheckIfButtonAvailable();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);

        EventManager.AddHandler(GameEvent.OnRotateHorizontal,OnRotateHorizontal);
        EventManager.AddHandler(GameEvent.OnRotateVertical,OnRotateVertical);
        EventManager.AddHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);
        EventManager.AddHandler(GameEvent.OnCleanCube,OnCleanCube);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCheckHelpers,OnCheckHelpers);
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);

        EventManager.RemoveHandler(GameEvent.OnRotateHorizontal,OnRotateHorizontal);
        EventManager.RemoveHandler(GameEvent.OnRotateVertical,OnRotateVertical);
        EventManager.RemoveHandler(GameEvent.OnCollect3Emoji,OnCollect3Emoji);
        EventManager.RemoveHandler(GameEvent.OnCleanCube,OnCleanCube);
    }

    private void OnCheckHelpers()
    {
        CheckIfButtonAvailable();
    }

    //Or OnGameStart
    private void OnNextLevel()
    {
        CheckIfButtonAvailable();
    }

    private void OnSuccess()
    {
        purchasePanel.SetActive(false);
    }

    private void OnFail()
    {
        purchasePanel.SetActive(false);
    }

    #region Boosters Particles

    private void OnRotateHorizontal()
    {
        PlayParticle(rotatesParticle);
    }

    private void OnRotateVertical()
    {
        PlayParticle(rotatesParticle);
    }

    private void OnCleanCube()
    {
        PlayParticle(cleanerParticle);
    }

    private void OnCollect3Emoji()
    {
        PlayParticle(collect3Particle);
    }

    private void PlayParticle(ParticleSystem particle)
    {
        particle.Play();
    }

    #endregion

    #region Booster Control
    private void AssignHelperImages()
    {
        for (int i = 0; i < helperProperties.Count; i++)
        {
            //helperProperties[i].HelperImage.sprite=helperProperties[i].helperConfig.HelperSprite;
            var helperProperty = helperProperties[i];
            var config = helperProperty.helperConfig;

            // Load the Amount value from PlayerPrefs, defaulting to 0 if not set
            config.Amount = PlayerPrefs.GetInt($"Helper_{i}_Amount", 0);
            helperProperty.UseAmountText.SetText(config.Amount.ToString());

            // Check lock status and set unlock level text
            bool isLocked = (gameData.levelNumber+1) < config.UnlockLevel;
            helperProperty.Locked.SetActive(isLocked);
            helperProperty.UnlockedLevelText.SetText($"Unlocks at Level {config.UnlockLevel}");
            helperProperty.UnlockedLevelText.gameObject.SetActive(isLocked);
        }
    }

    private void CheckIfButtonAvailable()
    {

        for (int i = 0; i < helperProperties.Count; i++)
        {
            var helperProperty = helperProperties[i];
            var config = helperProperty.helperConfig;
            var buyButton = helperProperty.BuyButton;
            var useButton = helperProperty.UseButton;
            var lockedButton=helperProperty.Locked;

             // Check if the helper is unlocked based on the level
            bool isUnlocked = (gameData.levelNumber+1) >= config.UnlockLevel;

            // Check if the helper has been bought
            bool hasAmount = config.Amount > 0;

            // Check if the score requirement is met
            bool canBuy = config.RequirementScore <= gameData.score;

            // Update lock panel visibility
            lockedButton.SetActive(!isUnlocked);

            // Update button visibility
            buyButton.gameObject.SetActive(isUnlocked && !hasAmount);
            useButton.gameObject.SetActive(isUnlocked && hasAmount);



            // Update button interactability
            /*buyButton.interactable = isUnlocked && !hasAmount && canBuy && !isUsingHelper;*/
            useButton.interactable = isUnlocked && hasAmount && !isUsingHelper;

            PlayerPrefs.SetInt($"Helper_{i}_Amount", config.Amount);
        }

        PlayerPrefs.Save();
    }


    //Use Purchase panel and helper panel effects in panel manager!!!!
    public void OpenPurchasePanel(int index)
    {
        purchasePanel.transform.localScale=Vector3.zero;
        purchasePanel.SetActive(true);
        purchasePanel.transform.DOScale(Vector3.one,.5f).SetEase(Ease.OutQuart);
        //EventManager.Broadcast(GameEvent.OnOpenPurchasePanel);
        headerText.SetText(helperProperties[index].PurchaseHeader);
        purchaseAmountText.SetText("x " + helperProperties[index].helperConfig.GivenAmount.ToString());
        briefText.SetText(helperProperties[index].PurchaseBrief);
        purchasePriceText.SetText(helperProperties[index].helperConfig.RequirementScore.ToString());
        purchaseIcon.sprite=helperProperties[index].PurchaseIcon;
        
        buyIndex=index;

         // Check if the helper is unlocked based on the level
        bool isUnlocked = (gameData.levelNumber+1) >= helperProperties[index].helperConfig.UnlockLevel;

        // Check if the helper has been bought
        bool hasAmount = helperProperties[index].helperConfig.Amount > 0;

        // Check if the score requirement is met
        bool canBuy = helperProperties[index].helperConfig.RequirementScore <= gameData.score;
        purchaseBuyButton.interactable = isUnlocked && !hasAmount && canBuy && !isUsingHelper;

        
    }

    public void ClosePurchasePanel()
    {
        //EventManager.Broadcast(GameEvent.OnClosePurchasePanel);
        purchasePanel.transform.DOScale(Vector3.zero,.5f).SetEase(Ease.OutQuart).OnComplete(()=>{
            purchasePanel.SetActive(false);
        });
        
    }

    public void BuyHelper()
    {
        if(gameData.score>=helperProperties[buyIndex].helperConfig.RequirementScore)
        {
            gameData.score-=helperProperties[buyIndex].helperConfig.RequirementScore;
            gameData.decreaseScore=helperProperties[buyIndex].helperConfig.RequirementScore;
            EventManager.Broadcast(GameEvent.OnScoreUIUpdate);
            PlayerPrefs.SetInt("Score",gameData.score);
            helperProperties[buyIndex].helperConfig.Amount=helperProperties[buyIndex].helperConfig.GivenAmount;
            helperProperties[buyIndex].UseAmountText.SetText(helperProperties[buyIndex].helperConfig.Amount.ToString());

            //EventManager.Broadcast(GameEvent.OnBuyButtonTap);
            
            CheckIfButtonAvailable();

            ClosePurchasePanel();
        }

        else
            return;
    }

    public void UseHelper(int index)
    {
        helperProperties[index].helperConfig.Amount--;
        helperProperties[index].UseAmountText.SetText(helperProperties[index].helperConfig.Amount.ToString());
        SetGameEvent(index);
        //EventManager.Broadcast(GameEvent.OnUseButtonTap);
        CheckIfButtonAvailable();
    }


    private void SetGameEvent(int eventIndex)
    {
        switch(eventIndex)
        {
            case 0:
                EventManager.Broadcast(GameEvent.OnRotateVertical);
                break;

            case 1:
                EventManager.Broadcast(GameEvent.OnRotateHorizontal);
                break;
            
            case 2:
                EventManager.Broadcast(GameEvent.OnCleanCube);
                break;

            case 3:
                EventManager.Broadcast(GameEvent.OnCollect3Emoji);
                break;
        }
    }

    #endregion


    
}

