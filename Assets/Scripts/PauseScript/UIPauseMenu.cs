using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    //Buat sambungin ke button lainnya 
    public GameFlowManager gameFlowManager;
    public Button buttonResume;
    public Button buttonExit;
    public Button pauseButton;

    private void Awake()
    {
        gameFlowManager = GameFlowManager.Instance;
    }

    private void Start()
    {
        buttonResume.onClick.AddListener(gameFlowManager.ResumeGame);
        buttonExit.onClick.AddListener(gameFlowManager.ExitGame);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
}
