using UnityEngine;

public class Death : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.transform.position = CheckpointManager.lastCheckpointPos;
        }
    }
}
