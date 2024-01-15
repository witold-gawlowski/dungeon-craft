using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private Image fadeoutImage;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Image flagFillImage;
    [SerializeField] private GameObject flagObject;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private GameObject runCompletedPanel;

    [SerializeField] private float fadeLength = 0.5f;
    [SerializeField] private float countdownDuration = 3.0f;
    
    public Action fadeEndedEvent;
    public Action giveUpEvent;

    private bool countDownOn;
    private float countdownTimer;
    private bool isFading;
    private float flagFillValue;
    private bool flagFilled;
    
    private void Awake()
    {
        MissionManager.Instance.Subscribe(this);

        MissionManager.Instance.scoreChangedEvent += SetScore;
        MissionManager.Instance.timeChangedEvent += SetTimer;
        
        countDownOn = false;
        isFading = false;
        flagFilled = false;
    }

    private void Start()
    {
        completionPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (countDownOn)
        {
            countdownTimer -= Time.deltaTime;
            UpdateCountdownText();
            if (countdownTimer < 0 && !isFading)
            {
                StartCoroutine(Fade());
                isFading = true;
            }
        }

        if (flagFillValue > 0 && !flagFilled)
        {
            flagFillValue -= Time.deltaTime * 0.15f;
            UpdateFlagUI();
        }

        if (flagFillValue < 0.1f && flagObject.activeInHierarchy)
        {
            flagObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleFlagPress();
        }
    }

    public void OnDestroy()
    {
        MissionManager.Instance.scoreChangedEvent -= SetScore;
        MissionManager.Instance.timeChangedEvent -= SetTimer;
        
        MissionManager.Instance.Unsubscribe(this);
    }

    public void HandleFlagPress()
    {
        flagObject.SetActive(true);
        
        if (flagFillValue > 0.8f)
        {
            flagFilled = true;
            giveUpEvent?.Invoke();
        }
        flagFillValue += 0.4f;
        UpdateFlagUI();
    }

    public void HandleMissionCompleted(bool RunIsFinished)
    {
        if(RunIsFinished)
        {
            ShowRunCompletedPanel();
        }
        else
        {
            ShowMapCompletedPanel();
        }
        BeginCountdownIntoFade(countdownDuration);
        scoreText.color = Color.green;
    }

    public void HandleMissionFailed()
    {
        BeginCountdownIntoFade(1.0f);
    }
    
    public void SetScore(int score, int target)
    {
        scoreText.text = score.ToString() + " / " + target.ToString();
    }

    public void SetTimer(float timeLeft)
    {
        var seconds = Mathf.RoundToInt(timeLeft + 0.5f);
        timeText.text =  string.Format("{0:0}:{1:00}", seconds / 60, seconds % 60);
    }

    private void BeginCountdownIntoFade(float countdownLenght)
    {
        countdownTimer = countdownLenght;
        countDownOn = true;
    }
    
    private void UpdateFlagUI()
    {
        flagFillImage.fillAmount = flagFillValue;
    }
    
    private void ShowMapCompletedPanel()
    {
        completionPanel.SetActive(true);
        var reward = BriefingManager.Instance.GetReward();
        rewardText.text = "+" + reward + "$";
    }

    private void ShowRunCompletedPanel()
    {
        runCompletedPanel.SetActive(true);
    }
    
    private void UpdateCountdownText()
    {
        countdownText.text = "Returning in " + Mathf.RoundToInt(countdownTimer + 0.5f);
    }
    private IEnumerator Fade()
    {
        fadeoutImage.gameObject.SetActive(true);
        Color c = fadeoutImage.color;
        float step = 0.01f;
        for (float alpha = 0; alpha <= 1.0f; alpha += step)
        {
            c.a = alpha;
            fadeoutImage.color = c;
            yield return new WaitForSeconds(fadeLength * step);
        }
        fadeEndedEvent?.Invoke();
    }
}
