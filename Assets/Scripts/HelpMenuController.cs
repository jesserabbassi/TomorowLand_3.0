using UnityEngine;
using UnityEngine.UIElements;

public class HelpMenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _helpPanel;
    private bool _isHelpVisible = false;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;

        // Find the panel by the name we gave it in the UXML
        _helpPanel = root.Q<VisualElement>("HelpPanel");

        // Start with the menu hidden
        if (_helpPanel != null)
        {
            _helpPanel.style.display = DisplayStyle.None;
        }
    }

    private void Update()
    {
        // Listen for the 'H' key
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (_helpPanel == null) return;

            // Toggle visibility flag
            _isHelpVisible = !_isHelpVisible;

            // Apply visibility
            if (_isHelpVisible)
            {
                _helpPanel.style.display = DisplayStyle.Flex;
            }
            else
            {
                _helpPanel.style.display = DisplayStyle.None;
            }
        }
    }
}