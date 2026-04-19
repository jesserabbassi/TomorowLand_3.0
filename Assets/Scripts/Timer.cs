using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f; // Time in seconds
    private float timer = 0f;
    public TextMeshProUGUI timetxt;
    public GameObject gameOverPanel;
    public TextMeshProUGUI statusText;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioSource statusAudioSource;
    public CarToRepair[] carsToRepair;
    public string winMessage = "You Win!";
    public string loseMessage = "You Lose!";

    private bool isFinished = false;

    private void Start()
    {
        timer = timeRemaining;
        if (statusAudioSource == null)
            statusAudioSource = GetComponent<AudioSource>();

        if (carsToRepair == null || carsToRepair.Length == 0)
            carsToRepair = FindObjectsOfType<CarToRepair>();

        EnsureStatusPanel();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
            return;

        if (AllCarsRepaired())
        {
            FinishTimer(true);
            return;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                UpdateTimerText();
                FinishTimer(false);
                return;
            }

            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (timetxt == null)
            return;

        int m = Mathf.FloorToInt(timer / 60);
        int s = Mathf.FloorToInt(timer % 60);
        timetxt.text = string.Format("{0:00}:{1:00}", m, s);
    }

    private bool AllCarsRepaired()
    {
        if (carsToRepair == null || carsToRepair.Length == 0)
            return false;

        bool hasRepairTarget = false;
        foreach (CarToRepair car in carsToRepair)
        {
            if (car == null)
                continue;

            hasRepairTarget = true;
            if (!car.isRepaired)
                return false;
        }

        return hasRepairTarget;
    }

    private void FinishTimer(bool won)
    {
        isFinished = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (statusText != null)
            statusText.text = won ? winMessage : loseMessage;

        PlayStatusSound(won ? winSound : loseSound);
        enabled = false; // Stop the timer
    }

    private void PlayStatusSound(AudioClip clip)
    {
        if (clip == null)
            return;

        if (statusAudioSource != null)
            statusAudioSource.PlayOneShot(clip);
        else
            AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void EnsureStatusPanel()
    {
        if (gameOverPanel != null)
        {
            if (statusText == null)
                statusText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>(true);

            return;
        }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
            return;

        gameOverPanel = new GameObject("Status Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        gameOverPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(420f, 160f);

        Image panelImage = gameOverPanel.GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.72f);

        GameObject textObject = new GameObject("Status Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(gameOverPanel.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(20f, 20f);
        textRect.offsetMax = new Vector2(-20f, -20f);

        statusText = textObject.GetComponent<TextMeshProUGUI>();
        statusText.text = "";
        statusText.color = Color.white;
        statusText.fontSize = 46f;
        statusText.fontStyle = FontStyles.Bold;
        statusText.alignment = TextAlignmentOptions.Center;
    }
}
