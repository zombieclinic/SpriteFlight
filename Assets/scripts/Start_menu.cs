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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          PlayRandom();

          var root = uiDocument.rootVisualElement;
          startButton = root.Q<Button>("StartLabel");

          startButton.clicked += OnStartClicked;
    }

    // Update is called once per frame
   private void OnStartClicked()
{
    SceneManager.LoadScene("sprite_flight");
}


    private void PlayRandom()
    {
        int randomIndex = Random.Range(0, musicTracks.Length);
        audioSource.clip = musicTracks[randomIndex];
        audioSource.Play();
    }
}
