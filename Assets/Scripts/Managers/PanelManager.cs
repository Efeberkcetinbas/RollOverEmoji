using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PanelManager : MonoBehaviour
{
    [SerializeField] private RectTransform StartPanel,ScenePanel,SuccessPanel,FailPanel;

    [SerializeField] private List<GameObject> SceneUIs=new List<GameObject>();
    [SerializeField] private List<GameObject> SuccessElements=new List<GameObject>();
    [SerializeField] private List<GameObject> FailElements=new List<GameObject>();
    [SerializeField] private List<GameObject> SpecialElements=new List<GameObject>();
    [SerializeField] private Image Fade;
    [SerializeField] private float sceneX,sceneY,oldSceneX,oldSceneY,duration;


    [Header("Homepage Settings")]
    [SerializeField] private List<GameObject> homepageElements=new List<GameObject>();
    [SerializeField] private List<GameObject> stickerElements=new List<GameObject>();
    [SerializeField] private List<GameObject> customizationElements=new List<GameObject>();
    public GameData gameData;

    private WaitForSeconds waitForSeconds,waitforSecondsSpecial;

    private void Start()
    {
        waitForSeconds=new WaitForSeconds(.25f);
        waitforSecondsSpecial=new WaitForSeconds(1);
    }

    private void OnEnable() 
    {
        EventManager.AddHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.AddHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.AddHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.AddHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.AddHandler(GameEvent.OnRestartLevel,OnRestartLevel);


    }


    private void OnDisable() 
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel,OnNextLevel);
        EventManager.RemoveHandler(GameEvent.OnSuccess,OnSuccess);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI,OnSuccessUI);
        EventManager.RemoveHandler(GameEvent.OnFailUI,OnFailUI);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,OnRestartLevel);


    }


    

    
    
    public void StartGame() 
    {
        gameData.isGameEnd=false;
        StartPanel.gameObject.SetActive(false);
        ScenePanel.gameObject.SetActive(true);
        SetSceneUIPosition(sceneX,sceneY);
        StartCoroutine(GetCanSwipe());

       
        StartCoroutine(SetElementsDotween(SpecialElements));
        EventManager.Broadcast(GameEvent.OnGameStart);
        
    }

    private IEnumerator GetCanSwipe()
    {
        yield return new WaitForSeconds(0.1f);
        gameData.CanSwipe=true;
    }
    
    private void OnRestartLevel()
    {
        FailPanel.gameObject.SetActive(false);
        StartCoroutine(Blink(Fade.gameObject,Fade));
        SetActivity(SceneUIs,true);
        StartCoroutine(SetElementsDotween(SpecialElements));
    }

    

    private void OnNextLevel()
    {
        
        SuccessPanel.gameObject.SetActive(false);
        StartCoroutine(Blink(Fade.gameObject,Fade));
        /*SetActivity(SceneUIs,true);
        StartCoroutine(SetElementsDotween(SpecialElements));*/
    }

   

    private IEnumerator Blink(GameObject gameObject,Image image)
    {
        
        gameObject.SetActive(true);
        image.color=new Color(0,0,0,1);
        image.DOFade(0,2f);
        StartPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);

        //SetSceneUIPosition(sceneX,sceneY);

    }

    private IEnumerator SetElementsDotween(List<GameObject> elements)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].transform.localScale=Vector3.zero;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            yield return waitForSeconds;
            elements[i].transform.DOScale(Vector3.one,duration);
        }
    }

    private void SetActivity(List<GameObject> list,bool val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(val);
        }
    }

    private void OnSuccess()
    {
        //SetActivity(SceneUIs,false);
        SetSceneUIPosition(oldSceneX,oldSceneY);
    }

     private void OnSuccessUI()
    {
        SuccessPanel.gameObject.SetActive(true);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(SuccessElements));
    }
  

    

    private void OnFailUI()
    {
        FailPanel.gameObject.SetActive(true);
        SetActivity(SceneUIs,false);
        StartCoroutine(SetElementsDotween(FailElements));
    }

    private void SetSceneUIPosition(float valX,float valY)
    {
        ScenePanel.DOAnchorPos(new Vector2(valX,valY),duration);
    }
    
    #region Homepage

    

    public void OpenStickers()
    {
        SetPage(stickerElements,homepageElements,customizationElements);
    }

    public void OpenCustomization()
    {
        SetPage(customizationElements,homepageElements,stickerElements);
    }

    public void OpenHomepage()
    {
        SetPage(homepageElements,stickerElements,customizationElements);
    }

    private void SetPage(List<GameObject> selectedPages,List<GameObject> otherPage1,List<GameObject> otherPage2)
    {
        for (int i = 0; i < selectedPages.Count; i++)
        {
            selectedPages[i].transform.localScale=Vector3.zero;
            selectedPages[i].SetActive(true);
            selectedPages[i].transform.DOScale(Vector3.one,0.5f).SetEase(Ease.OutBounce);
        }

        for (int i = 0; i < otherPage1.Count; i++)
        {
            otherPage1[i].SetActive(false);
        }

        for (int i = 0; i < otherPage2.Count; i++)
        {
            otherPage2[i].SetActive(false);
        }
    }

    #endregion

}
