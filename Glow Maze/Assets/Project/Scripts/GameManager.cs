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
        Invoke("RestartLevel", 0.4f);
        
        Debug.Log("GAME OVER!");
    }

    public void DeductDiamonds()
    {
        SettingsMenu.Instance.diamondsCount -= LevelManager.Instance.diamondstoRemove;
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
        Vibrations.Vibrate(800);
    }

    void OnApplicationQuit()
    {
       if(LevelManager.Instance.diamondstoRemove <= LevelManager.Instance.diamondPickUpCount)
       {
            DeductDiamonds();
       }

       PlayerPrefs.SetInt("Diamonds", SettingsMenu.Instance.diamondsCount);
       Debug.Log("SAVED DIAMONDS " + PlayerPrefs.GetInt("Diamonds"));
    }
}
