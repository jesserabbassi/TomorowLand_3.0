
using UnityEngine;


public class PlayerPick : MonoBehaviour
{
    [HideInInspector] public GameObject heldObj=null;
    public Transform HeldPoint;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.GetComponent<PickupObject>() && heldObj == null)
        {
            hit.collider.GetComponent<PickupObject>().rotateObject = false;
            hit.collider.transform.SetParent(HeldPoint);
            heldObj = hit.collider.gameObject;
            hit.collider.transform.localPosition = Vector3.zero;
            hit.collider.transform.localRotation = Quaternion.identity;
            hit.collider.GetComponent<Rigidbody>().isKinematic = true;
            hit.collider.GetComponent<Collider>().enabled = false;
            hit.collider.transform.localScale = Vector3.one;
            hit.collider.GetComponent<PickupObject>().transform.localScale = hit.collider.GetComponent<PickupObject>().originalScale*.5f;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && heldObj != null)
        {
            heldObj.GetComponent<PickupObject>().rotateObject = true;
            heldObj.transform.SetParent(null);
            heldObj.GetComponent<Rigidbody>().isKinematic = false;
            heldObj.GetComponent<Collider>().enabled = true;
            heldObj.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);
            heldObj.GetComponent<PickupObject>().transform.localScale = heldObj.GetComponent<PickupObject>().originalScale;
            heldObj = null;
        }
    }



}
