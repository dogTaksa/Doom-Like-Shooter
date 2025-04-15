using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float amplitude = 2f;
    public float speed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }
}
