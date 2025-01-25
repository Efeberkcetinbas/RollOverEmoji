using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum MonsterPartType
{
    Antenna,
    Arm,
    Body,
    Eye,
    Mouth,
    Eyebrow,
    Nose
}

public class MonsterCustomizer : MonoBehaviour
{
    [System.Serializable]
    private class MonsterPart
    {
        public MonsterPartType Type;
        public List<Sprite> Sprites;
        public List<Image> CurrentPartImages;
        public List<Image> ScenePartImages;
    }

    [Header("Monster Parts")]
    [SerializeField] private List<MonsterPart> monsterParts;
    [SerializeField] private TextMeshProUGUI currentPartText;
    [SerializeField] private List<Button> colorButtons;
    [SerializeField] private List<Button> eyeOptionButtons; // For one eye, two eyes, three eyes

    private int currentPartIndex = 0;
    private Dictionary<MonsterPartType, int> partIndices = new Dictionary<MonsterPartType, int>();

    private void Start()
    {
        foreach (var part in monsterParts)
        {
            int savedIndex = PlayerPrefs.GetInt(part.Type.ToString(), 0);
            partIndices[part.Type] = savedIndex;

            foreach (var image in part.CurrentPartImages)
                image.color = Color.white; // Default color

            foreach (var image in part.ScenePartImages)
                image.color = Color.white; // Default color

            UpdatePartSprites(part.Type);
        }
        UpdateCurrentPartText();
        UpdateEyeButtons();
    }

    public void ChangeMonsterPart(bool isNext)
    {
        currentPartIndex = (currentPartIndex + (isNext ? 1 : -1) + monsterParts.Count) % monsterParts.Count;
        UpdateCurrentPartText();
        UpdateEyeButtons();
    }

    public void ChangePartSprite(bool isNext)
    {
        var currentPart = monsterParts[currentPartIndex];
        partIndices[currentPart.Type] = (partIndices[currentPart.Type] + (isNext ? 1 : -1) + currentPart.Sprites.Count) % currentPart.Sprites.Count;

        PlayerPrefs.SetInt(currentPart.Type.ToString(), partIndices[currentPart.Type]);
        PlayerPrefs.Save();

        UpdatePartSprites(currentPart.Type);
    }

    public void ChangePartColor(string colorName)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorName, out color))
        {
            var currentPart = monsterParts[currentPartIndex];
            foreach (var image in currentPart.CurrentPartImages)
            {
                image.color = color;
            }
            foreach (var image in currentPart.ScenePartImages)
            {
                image.color = color;
            }
        }
        else
        {
            Debug.LogError($"Invalid color name: {colorName}");
        }
    }

    public void SetEyeOption(int eyeCount)
    {
        var eyePart = monsterParts.Find(p => p.Type == MonsterPartType.Eye);
        if (eyePart == null) return;

        // Ensure eye order: LeftEye, RightEye, MiddleEye
        for (int i = 0; i < eyePart.CurrentPartImages.Count; i++)
        {
            var isActive = (eyeCount == 1 && i == 2) || (eyeCount == 2 && i < 2) || (eyeCount == 3);
            eyePart.CurrentPartImages[i].gameObject.SetActive(isActive);
        }
        for (int i = 0; i < eyePart.ScenePartImages.Count; i++)
        {
            var isActive = (eyeCount == 1 && i == 2) || (eyeCount == 2 && i < 2) || (eyeCount == 3);
            eyePart.ScenePartImages[i].gameObject.SetActive(isActive);
        }

        PlayerPrefs.SetInt("EyeCount", eyeCount);
        PlayerPrefs.Save();
    }

    private void UpdatePartSprites(MonsterPartType type)
    {
        var part = monsterParts.Find(p => p.Type == type);
        if (part == null) return;

        int index = partIndices[type];
        foreach (var image in part.CurrentPartImages)
        {
            image.sprite = part.Sprites[index];
        }

        foreach (var image in part.ScenePartImages)
        {
            image.sprite = part.Sprites[index];
        }
    }

    private void UpdateCurrentPartText()
    {
        currentPartText.text = monsterParts[currentPartIndex].Type.ToString();
    }

    private void UpdateEyeButtons()
    {
        bool isEyePart = monsterParts[currentPartIndex].Type == MonsterPartType.Eye;
        foreach (var button in eyeOptionButtons)
        {
            button.gameObject.SetActive(isEyePart);
        }
    }
}

