using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class BriefingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    
    [SerializeField] private GameObject emptyLevelInventoryText;
    [SerializeField] private Transform levelInventoryParent;
    [SerializeField] private LevelButtonScript selectedLevelButton;
    [SerializeField] private Slider targetSlider;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private GameObject briefingPanel;
    [SerializeField] private GameObject emptySelectionPanel;

    public Action<float> targetUpdatedEvent;
    public Action<MapConfig> levelButtonPressed;
    
    private void Awake()
    {
        BriefingManager.Instance.Subscribe(this);
        LevelInventory.Instance.Subscribe(this);
        BriefingManager.Instance.dataChanged += HandleDataChanged;
        LevelInventory.Instance.dataChanged += HandleDataChanged;
    }

    private void Start()
    {
        HandleDataChanged();
    }

    private void OnDestroy()
    {
        BriefingManager.Instance.Unsubscribe(this);
        LevelInventory.Instance.Unsubscribe(this);
        BriefingManager.Instance.dataChanged -= HandleDataChanged;
        LevelInventory.Instance.dataChanged -= HandleDataChanged;
    }

    public void HandleSliderValueChanged()
    {
        var value = targetSlider.value;
        targetUpdatedEvent?.Invoke(value);
    }
    
    private void HandleDataChanged()
    {
        emptyLevelInventoryText.SetActive(false);
        CreateLevelInventory();
        UpdateLevelSelection();
        UpdateSlider();
    }
    
    private void UpdateLevelSelection()
    {
        var selectedLevel = BriefingManager.Instance.GetSelectedMap();
        if (selectedLevel != null)
        {
            emptySelectionPanel.SetActive(false);
            briefingPanel.SetActive(true);
            selectedLevelButton.InitSelectionButton(selectedLevel.GetSprite(), 0, false, false, false);
        }
        else
        {
            briefingPanel.SetActive(false);
            emptySelectionPanel.SetActive(true);
        }
    }

    private void UpdateSlider()
    {
        var selectedLevelConfig = BriefingManager.Instance.GetSelectedMap();
        if (selectedLevelConfig != null)
        {
            var target = BriefingManager.Instance.GetTarget();
            var reward = BriefingManager.Instance.GetReward();
            var levelSize = Helpers.GetLevelSize(selectedLevelConfig.levelPrefab);
            targetSlider.value = 1.0f * target / levelSize;
            targetText.text = target + " / " + levelSize;
            rewardText.text = reward + "$";
        }
    }
    
    private void CreateLevelInventory()
    {
        ClearLevelInventory();
        var levels = LevelInventory.Instance.GetLevels();
        foreach (var level in levels)
        {
            var newButton = Instantiate(levelButtonPrefab, levelInventoryParent);
            var buttonScript = newButton.GetComponent<LevelButtonScript>();
            var selectedLevel = BriefingManager.Instance.GetSelectedMap();
            var isSelected = selectedLevel == level.MapConfig;
            
            buttonScript.InitSelectionButton(level.MapConfig.GetSprite(), 0, isSelected, level.IsNew, false);
            buttonScript.buttonPressed += delegate { levelButtonPressed?.Invoke(level.MapConfig); };
        }
    }

    private void ClearLevelInventory()
    {
        foreach (Transform t in levelInventoryParent.transform)
        {
            if (t != levelInventoryParent.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
