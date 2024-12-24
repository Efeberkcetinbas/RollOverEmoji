using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComboManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    [Header("Combo Settings")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float intervalDecrement = 1f;
    [SerializeField] private List<GameObject> comboList=new List<GameObject>();
    
    [Header("UI Elements to Randomize")]
    [SerializeField] private List<RectTransform> posUIElements;
    [SerializeField] private List<RectTransform> colorUIElements; // Assign UI Images/Texts RectTransforms
    [SerializeField] private List<Color> colorList;          // Predefined list of colors

    

   
    private bool isComboActive;

    private void Start()
    {
        gameData.currentInterval=20;
        gameData.comboCount=0;
    }

    

    private void Update()
    {
        if (!isComboActive) return;

        gameData.elapsedTime += Time.deltaTime;
        EventManager.Broadcast(GameEvent.OnComboProgress);

        if (gameData.elapsedTime >= gameData.currentInterval)
        {
            ResetCombo();
        }
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnMatch, OnMatch);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnFail,OnFail);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnMatch, OnMatch);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnFail,OnFail);
    }

    private void OnMatch()
    {
        SetActivity(true);
        gameData.comboCount++;
        gameData.elapsedTime = 0;
        isComboActive = true;

        EventManager.Broadcast(GameEvent.OnComboUIUpdate);
        EventManager.Broadcast(GameEvent.OnCollectCoin);

        //RandomizeUIElements();
        ApplyColor();

        // Adjust interval for increasing difficulty
        /*if (gameData.currentInterval > minInterval)
        {
            gameData.currentInterval -= intervalDecrement;
        }*/
    }

    private void OnSuccess()
    {
        ResetCombo();
    }

    private void OnFail()
    {
        ResetCombo();
    }

    

    private void ResetCombo()
    {
        gameData.comboCount = 0;
        gameData.elapsedTime = 0;
        isComboActive = false;
        EventManager.Broadcast(GameEvent.OnComboUIUpdate);
        SetActivity(false);
        
    }

    private void SetActivity(bool val)
    {
        for (int i = 0; i < comboList.Count; i++)
        {
            comboList[i].SetActive(val);
        }
    }

   

    private void ApplyColor()
    {
        Color randomColor = GetRandomColor();

        foreach (RectTransform element in colorUIElements)
        {
            // Set Random Color (Same for Image and Text)
            ApplyColorToElement(element, randomColor);
        }
    }

    /// <summary>
    /// Returns a random color from the predefined list.
    /// </summary>
    /// <returns>Color</returns>
    private Color GetRandomColor()
    {
        if (colorList.Count > 0)
        {
            int randomIndex = Random.Range(0, colorList.Count);
            return colorList[randomIndex];
        }
        return Color.white; // Default to white if no colors are defined
    }

    /// <summary>
    /// Applies the same color to Image and Text components within the UI element.
    /// </summary>
    /// <param name="element">RectTransform of the UI element</param>
    /// <param name="color">Color to apply</param>
    private void ApplyColorToElement(RectTransform element, Color color)
    {
        // Check and set color for Image component
        if (element.TryGetComponent<Image>(out Image imageComponent))
        {
            imageComponent.color = color;
        }

        // Check and set color for Text component
        if (element.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI textComponent))
        {
            textComponent.color = color;
        }
    }

}
