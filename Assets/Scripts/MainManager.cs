using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;
    public Text preScoreText;

    public string preName;
    public int prePlayerScore;

    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] {1, 1, 2, 2, 5, 5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            preName = data.playerName;
            prePlayerScore = data.score;
            preScoreText.text = $"Best Score : {preName} : {prePlayerScore}";
        }
    }


    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                LoadScore();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        SaveScore(MenuManager.Instance.pName, m_Points);
    }

    public void SaveScore(string playerName, int point)
    {
        Debug.Log("nameL:" + playerName + "points:" + m_Points);
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.score = point;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}

[System.Serializable]
class SaveData
{
    public string playerName;
    public int score;
}