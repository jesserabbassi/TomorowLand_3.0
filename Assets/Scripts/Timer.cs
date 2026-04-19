using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f; // Time in seconds
    float timer = 0f;
    public TextMeshProUGUI timetxt;

    private void Start()
    {
        timer = timeRemaining;
    }

    // Update is called once per frame
    void Update()
    {

        if (timer >= 0)
        {

            timer -= Time.deltaTime;
            int m = Mathf.FloorToInt(timer / 60);
            int s = Mathf.FloorToInt(timer % 60);
            timetxt.text = string.Format("{0:00}:{1:00}", m, s);
        }
        else
        {
            timetxt.text = "00:00";


        }
    }
}
