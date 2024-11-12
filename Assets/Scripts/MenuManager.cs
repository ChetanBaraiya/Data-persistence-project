using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public TMP_InputField playerName;

    public TMP_Text preScoreText;
    public string pName;
    public int playerScore;

    private void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    public void ClickOnStart()
    {
        GetName();
        SceneManager.LoadScene("Main");
    }

    public void GetName()
    {
        string pName = playerName.text;
        Debug.Log("Pname:" + pName);
        SaveData data = new SaveData();
        data.playerName = pName;
        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            pName = data.playerName;
            playerScore = data.score;
            preScoreText.text = $"Best Score : {pName} : {playerScore}";
        }
    }

    public void ClickOnQuit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}