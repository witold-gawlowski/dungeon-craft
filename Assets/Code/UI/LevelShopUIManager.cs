using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelShopUIManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelsParent;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TMP_Text dayAnnoucementText;

    [SerializeField] private float fadeLength;

    public Action<LevelShopManager.ShopItem> levelBouttonPressed;

    public Action fadePlayedEvent;
    public Action dayAnnouncmentShowEvent;
    
    private void Awake()
    {
        LevelShopManager.Instance.dataChangedEvent += RegenerateUI;
        LevelShopManager.Instance.Subscribe(this);
        
        StageManager.Instance.Subscribe(this);
    }

    private void Start()
    {
        RegenerateUI();
        
        if (!LevelShopManager.Instance.GetFadePlayed())
        {
            StartFade();
            fadePlayedEvent?.Invoke();
        }

        if (!LevelShopManager.Instance.GetDayAnnouncementShown())
        {
            ShowDayAnnouncmentTExt();
        }
    }
    
    private void OnDestroy()
    {
        LevelShopManager.Instance.dataChangedEvent -= RegenerateUI;
        LevelShopManager.Instance.Unsubscribe(this);

        StageManager.Instance.Unsubscribe(this);
    }

    private void RegenerateUI()
    {
        Clear();
        var shopItems = LevelShopManager.Instance.GetShopItems();
        var completerMaps = LevelShopManager.Instance.GetCompleterMaps();

        foreach (var item in shopItems)
        {
            var newButton = Instantiate(levelButtonPrefab, levelsParent);
            var buttonScript = newButton.GetComponent<LevelButtonScript>();
            var isCompleter = completerMaps.Contains(item.Map);
            buttonScript.InitShopButton(item.Map.GetSprite(), item.Price, 0, item.Bought, isCompleter);
            buttonScript.buttonPressed += delegate { levelBouttonPressed?.Invoke(item); };
        }
    }
    
    private void Clear()
    {
        foreach (Transform t in levelsParent.transform) 
        {
            if (t != levelsParent.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }

    private IEnumerator HideDayAnnoucementTextDelayed()
    {
        yield return new WaitForSeconds(2.0f);
        
        Color c = dayAnnoucementText.color;
        float step = 0.01f;
        for (float alpha = 1; alpha >= 0; alpha -= step)
        {
            c.a = alpha;
            dayAnnoucementText.color = c;
            yield return new WaitForSeconds(1.0f * step);
        }
        
        dayAnnoucementText.gameObject.SetActive(false);
    }

    private void ShowDayAnnouncmentTExt()
    {
        dayAnnoucementText.gameObject.SetActive(true);
        
        var number = DayCycleManager.Instance.GetDay();
        dayAnnoucementText.text = "DAY " + (number + 1);
        StartCoroutine(HideDayAnnoucementTextDelayed());
        
        dayAnnouncmentShowEvent?.Invoke();
    }
    
    private void StartFade()
    {
        StartCoroutine(Fade());
    }
    
    private IEnumerator Fade()
    {
        Color c = fadeImage.color;
        float step = 0.01f;
        for (float alpha = 1; alpha >= 0; alpha -= step)
        {
            c.a = alpha;
            fadeImage.color = c;
            yield return new WaitForSeconds(fadeLength * step);
        }
    }
}
