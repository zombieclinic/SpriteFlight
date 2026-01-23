using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
     
    public static GameManager Instance;

     [Header("UI")]
    public UIDocument uIDocument;
    private Label scoreText;
    private Button restartButton;

    private Label ScoreHudText;

    [Header("Score")]
    public float scoreMultiplier = 10f;

    private float score = 0f;
    private bool isGameOver = false;

    [Header("PlayerPrefs")]
    private const string HighScoreKey = "HighScore";
    private Label highScoreText;
    private int highScore = 0;

    [Header("Music")]
    public AudioSource audioSource;
    public AudioClip[] musicTracks;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreBoardStart();

        highScoreStart();

        PlayRandom();
        
    }

    // Update is called once per frame
    void Update()
    {
       

        if (!audioSource.isPlaying)
        {
            PlayRandom();
        }
        
    }


    public void GameOver()
    {
        isGameOver = true;
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;
        ScoreHudText.style.display = DisplayStyle.Flex;
        scoreText.style.display = DisplayStyle.None;
        int currentScore = Mathf.FloorToInt(score);
        ScoreHudText.text = "Score: " + score;

        if(currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();

            highScoreText.text = "High Score " + highScore;
        }
    }

     private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void scoreBoardStart()
    {
        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        ScoreHudText = uIDocument.rootVisualElement.Q<Label>("ScoreHubLabel");

        //restart
        restartButton = uIDocument.rootVisualElement.Q<Button>("RestartButton");
        ScoreHudText.style.display = DisplayStyle.None;
        restartButton.style.display = DisplayStyle.None;
      
        restartButton.clicked += ReloadScene;
    }

    private void highScoreStart()
    {
        highScoreText = uIDocument.rootVisualElement.Q<Label>("HighScoreLabel");

        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        highScoreText.text = "High Score: " + highScore;

        highScoreText.style.display = DisplayStyle.None;
    }


    private void PlayRandom()
    {
        int randomIndex = Random.Range(0, musicTracks.Length);
        audioSource.clip = musicTracks[randomIndex];
        audioSource.Play();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
     score += amount;
    scoreText.text = "Score: " + score;
    }
}
