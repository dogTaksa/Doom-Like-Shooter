using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static Vector3 lastCheckpointPos;

    void Start()
    {
        lastCheckpointPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
