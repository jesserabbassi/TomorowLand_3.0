using UnityEngine;

public class CarToRepair : MonoBehaviour
{
    public bool isRepaired = false;
    public GameObject repairingTool;
    public GameObject effect;
    public GameObject wheelPoint, wheel;
    public void Repair(GameObject tool)
    {
        if(isRepaired)
            return;
        if(tool != repairingTool)
            return;
        if(effect != null)
            effect.SetActive(false);
        isRepaired = true;
        if(wheelPoint != null)
        {
            wheel.transform.SetParent(null);wheel.transform.localScale = wheel.GetComponent<PickupObject>().originalScale;
            wheel.transform.rotation = Quaternion.identity;
            wheel.transform.position = wheelPoint.transform.position;
        }

    }
}
