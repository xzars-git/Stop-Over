using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFloorChanger : MonoBehaviour
{
    public Item itemBehaviour;
    public GameFlowManager gameFlowManager;

    [Header("Floor")]
    public Button upperFloor;
    public Button lowerFloor;

    [Header("Resume Button")]
    public Button resumeButton;

    private void Awake()
    {
        gameFlowManager = GameFlowManager.Instance;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Start()
    {
        upperFloor.onClick.AddListener(gameFlowManager.UpperFloor);
        lowerFloor.onClick.AddListener(gameFlowManager.LowerFloor);
        resumeButton.onClick.AddListener(gameFlowManager.ResumeGame);
    }
}
