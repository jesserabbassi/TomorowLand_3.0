using UnityEngine;
using UnityEngine.UIElements; // Needed for UI Toolkit
using UnityEngine.SceneManagement; // Needed to load different levels

public class MainMenuManager : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _playButton;
    private Button _exitButton;

    private void OnEnable()
    {
        // 1. Grab the UI Document component
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;

        // 2. Find the buttons by the exact names you set in UI Builder
        _playButton = root.Q<Button>("PlayButton");
        _exitButton = root.Q<Button>("ExitButton");

        // 3. Tell the buttons what to do when clicked
        if (_playButton != null)
        {
            _playButton.clicked += StartGame;
        }

        if (_exitButton != null)
        {
            _exitButton.clicked += QuitGame;
        }
    }

    // This runs when PLAY is clicked
    private void StartGame()
    {
        Debug.Log("Loading the city...");
        // This loads your specific scene. Make sure the spelling matches exactly!
        SceneManager.LoadScene("Demo_Scene_1");
    }

    // This runs when EXIT is clicked
    private void QuitGame()
    {
        Debug.Log("Exiting the game...");

        // Application.Quit() closes the actual built game.
        Application.Quit();

        // (Optional) If you want it to stop playing inside the Unity Editor too:
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}