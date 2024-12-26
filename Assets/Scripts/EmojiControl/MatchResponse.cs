using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MatchResponse : MonoBehaviour
{   
    [SerializeField] private GameObject matchPanel;
    [SerializeField] private Ease ease;
    [SerializeField] private List<Vector2> initialPos=new List<Vector2>();  
    public List<Image> MatchImages=new List<Image>();

    private WaitForSeconds waitForSeconds;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(1.5f);
    }


    internal void AssignMatchingImages(Sprite matchSprite)
    {
        matchPanel.SetActive(true);

        for (int i = 0; i < MatchImages.Count; i++)
        {
            MatchImages[i].transform.localScale=Vector3.zero;
            MatchImages[i].transform.DOScale(Vector3.one,.25f).SetEase(ease);
            MatchImages[i].sprite=matchSprite;
            MatchImages[i].rectTransform.DOAnchorPos(Vector2.zero,1f).SetEase(ease);
        }

        StartCoroutine(SetMatch());


    }

    private IEnumerator SetMatch()
    {
        yield return waitForSeconds;

        for (int i = 0; i < MatchImages.Count; i++)
        {
            MatchImages[i].rectTransform.anchoredPosition=initialPos[i];
        }

        matchPanel.SetActive(false);
    }
}
