using UnityEngine;
using UnityEngine.UI;

public class CarEnterExit : MonoBehaviour
{
    public PrometeoCarController prometeoCarController;
    public Transform exitPoint;
    public float fuel = 100f;
    public float fuelSpeed = 0.1f;
    public float fallbackExitDistance = 4f;
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
        if (isInCar && prometeoCarController != null && prometeoCarController.carSpeed > 0.1f)
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
        if (player == null || prometeoCarController == null) return;

        prometeoCarController.enabled = true;
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
        player.transform.SetParent(prometeoCarController.transform, true);
    }

    void ExitCar()
    {
        if (player == null) return;

        isInCar = false;

        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
            characterController.enabled = false;

        // Detach player before moving them out of the car.
        player.transform.SetParent(null, true);
        player.transform.position = GetExitPosition();

        if (characterController != null)
            characterController.enabled = true;

        // Enable player movement
        var movement = player.GetComponent<playermvt>();
        if (movement != null)
            movement.enabled = true;

        // Show player mesh
        var mesh = player.GetComponentInChildren<SkinnedMeshRenderer>();
        if (mesh != null)
            mesh.enabled = true;

        // Disable car control
        if (prometeoCarController != null)
            prometeoCarController.enabled = false;

        isNearCar = false;
    }

    Vector3 GetExitPosition()
    {
        if (exitPoint != null && exitPoint != transform)
            return exitPoint.position;

        Transform carTransform = prometeoCarController != null ? prometeoCarController.transform : transform;
        return carTransform.position + carTransform.right * fallbackExitDistance;
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
