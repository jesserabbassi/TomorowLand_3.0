using UnityEngine;
using UnityEngine.UI;

public class CarEnterExit : MonoBehaviour
{
    public PrometeoCarController prometeoCarController;
    public Transform exitPoint;
    public float fuel = 100f;
    public float fuelSpeed = 0.1f;
    public Slider fuelBar;

    private GameObject player;

    // TRUE when player is in trigger zone (near car)
    private bool isNearCar = false;

    // TRUE when player is actually inside the car
    private bool isInCar = false;

    void Update()
    {
        // ENTER CAR
        if (!isInCar && isNearCar && Input.GetKeyDown(KeyCode.E))
        {
            EnterCar();
        }

        // EXIT CAR
        if (isInCar && Input.GetKeyDown(KeyCode.F))
        {
            ExitCar();
        }

        // FUEL CONSUMPTION
        if (isInCar && prometeoCarController.carSpeed > 0.1f)
        {
            fuel -= prometeoCarController.carSpeed * Time.deltaTime * fuelSpeed;
            fuel = Mathf.Clamp(fuel, 0f, 100f);

            if (fuelBar != null)
                fuelBar.value = fuel / 100f;

            if (fuel <= 0f)
            {
                prometeoCarController.enabled = false;
            }
        }
    }

    void EnterCar()
    {
        if (player == null) return;

        isInCar = true;

        // Disable player movement
        var movement = player.GetComponent<playermvt>();
        if (movement != null)
            movement.enabled = false;

        // Hide player mesh
        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh != null)
            mesh.enabled = false;

        // Attach player to car
        player.transform.SetParent(transform);

        // Enable car control
        prometeoCarController.enabled = true;
    }

    void ExitCar()
    {
        if (player == null) return;

        isInCar = false;

        // Enable player movement
        var movement = player.GetComponent<playermvt>();
        if (movement != null)
            movement.enabled = true;

        // Show player mesh
        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh != null)
            mesh.enabled = true;

        // Detach player
        player.transform.SetParent(null);

        // Move player to exit point
        if (exitPoint != null)
            player.transform.position = exitPoint.position;
        else
            player.transform.position = transform.position + new Vector3(3, 0, 0);

        // Disable car control
        prometeoCarController.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isNearCar = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearCar = false;
        }
    }
}
