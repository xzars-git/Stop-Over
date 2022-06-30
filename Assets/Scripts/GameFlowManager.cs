using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("UI")]
    public UIPauseMenu PauseMenuUI;

    [Header("FloorUI")]
    public UIFloorChanger FloorChangerUI;

    [Header("Scene")]
    public Object upperFloor;
    public Object lowerFloor;

    [Header("Transition")]
    public LevelLoader levelLoader;

    // transition animation
    public Animator transition;
    public float transitionTime = 1f;


    #region Singleton

    private static GameFlowManager _instance = null;

    public static GameFlowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameFlowManager>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: GameFlowManager not Found");
                }
            }
            return _instance;
        }
    }

    #endregion

    public bool IsGameOver { get { return isGameOver; } }

    private bool isGameOver = false;

    private void Start()
    {
        isGameOver = false;
        PauseMenuUI.gameObject.SetActive(false);
        FloorChangerUI.gameObject.SetActive(false);
        levelLoader.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; 
        PauseMenuUI.Show();
/*        SoundManager.Instance.PlayPause();*/
    }

    public void stop()
    {
        Time.timeScale = 0f;
    }

    public void run()
    {
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; 
        PauseMenuUI.Hide();
        FloorChangerUI.Hide();
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void UpperFloor()
    {
        StartCoroutine(LoadLevel(upperFloor.name));
        Time.timeScale = 1f;
        FloorChangerUI.Hide();
    }

    public void LowerFloor()
    {
        StartCoroutine(LoadLevel(lowerFloor.name));
        Time.timeScale = 1f;
        FloorChangerUI.Hide();
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);

    }
}
