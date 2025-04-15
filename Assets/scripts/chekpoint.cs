using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CheckpointManager.lastCheckpointPos = col.gameObject.transform.position;
        }
    }
}
