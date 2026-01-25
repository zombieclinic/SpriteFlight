using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Start_menu : MonoBehaviour
{

    [Header("Music")]
    public AudioSource audioSource;
    public AudioClip[] musicTracks;

    public UIDocument uiDocument;

    [Header("Buttons")]
    private Button startButton;
    private Button exitButton;
    public AudioSource ButtonAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayRandom();

        var root = uiDocument.rootVisualElement;
        startButton = root.Q<Button>("StartLabel");
        exitButton = root.Q<Button>("ExitLabel");

        startButton.clicked += OnStartClicked;
        exitButton.clicked += OnExitClicked;
    }

    // Update is called once per frame
    private void OnStartClicked()
    {
        ButtonAudio.Play();
        Invoke(nameof(LoadGame), 1f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("sprite_flight");
    }

    private void OnExitClicked()
    {
        ButtonAudio.Play();
        Invoke(nameof(LoadQuit), 1f);
    }

    private void LoadQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlayRandom()
    {
        int randomIndex = Random.Range(0, musicTracks.Length);
        audioSource.clip = musicTracks[randomIndex];
        audioSource.Play();
    }
}
