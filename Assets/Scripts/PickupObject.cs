using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]


public class PickupObject : MonoBehaviour
{

    
    [Header("Float Settings")]
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatSpeed = 1.5f;

    [Header("Rotation Settings")]
    [HideInInspector]public bool rotateObject = true;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);

    private Vector3 startPosition;

    public Vector3 originalScale;

    private void Start()
    {
        startPosition = transform.position;
            originalScale = transform.localScale;
    }

    private void Update()
    {

        if (!rotateObject)
        {
            return;
        }
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}