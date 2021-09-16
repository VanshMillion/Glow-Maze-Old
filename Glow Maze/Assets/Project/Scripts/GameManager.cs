using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioMixer mainMixer;

    private int savedLevel;

    public GameObject gameOverPanel;

    public bool isLevelCompleted;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        savedLevel = PlayerPrefs.GetInt("SavedLevel"); //Save current Level for later use
        Debug.Log("STARTED SAVED LEVEL! " + savedLevel);
        SceneManager.LoadScene(savedLevel); //Load game from savedLevel
    }

    void Update()
    {
        DeviceBackFunction();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Invoke("DeductDiamonds", 0.3f);
        Invoke("OpenGameOverPanel", 0.4f);
        
        Debug.Log("GAME OVER!");
    }

    public void DeductDiamonds()
    {
        if(SettingsMenu.Instance.diamondsCount > 0)
        {
            SettingsMenu.Instance.diamondsCount -= LevelManager.Instance.diamondstoRemove;
        }
    }

    void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void CloseGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }

    public void VibrateOnMove()
    {
        if (SettingsMenu.Instance.isVibrationOn)
        {
            Vibrations.Vibrate(50);
        }
    }

    public void VibrateOnWinorLose()
    {
        if (SettingsMenu.Instance.isVibrationOn)
        {
            Vibrations.Vibrate(600);
        }
    }

    public void VibrateOnButtonPress()
    {
        Vibrations.Vibrate(200);
    }

    void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount)
            {
                DeductDiamonds();
            }

            PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
            Debug.Log("SAVED DIAMONDS IN ANDROID " + PlayerPrefs.GetInt("Diamonds"));
        }

#if UNITY_EDITOR
        if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount)
        {
            DeductDiamonds();
        }

        PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
        Debug.Log("SAVED DIAMONDS IN EDITOR " + PlayerPrefs.GetInt("Diamonds"));
#endif
    }

    void DeviceBackFunction()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                //Time.timeScale = 0; //Pause game in Background
            }
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            //Time.timeScale = 0; //Pause game in Background
        }
#endif
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount)
            {
                DeductDiamonds();
            }

            PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
            PlayerPrefs.Save();
        }
    }
}
