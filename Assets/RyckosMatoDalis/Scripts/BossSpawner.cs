using UnityEngine;

public class EnableOnTouch : MonoBehaviour
{
    public GameObject objectToEnable;
    public string triggeringTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag) && objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}