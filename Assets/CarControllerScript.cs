using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CarEnterExit : MonoBehaviour
{
    public PrometeoCarController prometeoCarController;
    public Transform exitPoint;

    private GameObject player;
    private bool isInside = false;

    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            EnterCar();
        }
        else if (!isInside && player != null && Input.GetKeyDown(KeyCode.F))
        {
            ExitCar();
        }
    }

    void EnterCar()
    {
        player.GetComponent<playermvt>().enabled = false;
        player.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        player.transform.SetParent(transform);
       // player.SetActive(false); // hide player
        prometeoCarController.enabled = true;

        isInside = false;
    }

    void ExitCar()
    {
        player.transform.SetParent(null);
        player.GetComponent<playermvt>().enabled = true;
        player.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        player.transform.position = new Vector3(5,0,0)+ prometeoCarController.transform.position;
        
      //  player.SetActive(true);

        prometeoCarController.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("hhhhhh");
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
        }
    }
}