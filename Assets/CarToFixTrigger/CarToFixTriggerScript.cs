using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarRepairTrigger : MonoBehaviour
{
    public GameObject repairUI;
    public Slider progressBar;
    public float repairTime = 3f;

    private bool playerInside = false;
    private bool isRepairing = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !isRepairing)
        {
            StartCoroutine(RepairProcess());
        }
    }

    IEnumerator RepairProcess()
    {
        isRepairing = true;
        repairUI.SetActive(true);

        float elapsed = 0f;

        while (elapsed < repairTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / repairTime;

            progressBar.value = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        progressBar.value = 1f;

        repairUI.SetActive(false);
        isRepairing = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}