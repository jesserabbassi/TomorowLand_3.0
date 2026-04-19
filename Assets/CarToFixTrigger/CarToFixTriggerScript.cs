using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarRepairTrigger : MonoBehaviour
{
    public GameObject repairUI;
    public CarToRepair carToRepair;
    public float repairTime = 3f;

    private bool playerInside = false;
    private bool isRepairing = false;

    private void Start()
    {
        
        repairUI.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !isRepairing && !carToRepair.isRepaired)
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

            repairUI.GetComponent<Slider>().value = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        repairUI.GetComponent<Slider>().value = 1f;

        repairUI.SetActive(false);
        isRepairing = false;
        carToRepair.Repair(FindFirstObjectByType<PlayerPick>().heldObj);
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