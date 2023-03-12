using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject FollowActor;
    [SerializeField] Vector3 CameraOffset = new Vector3(0, 0, -10);

    // Start is called before the first frame update
    void Start()
    {
        if (!FollowActor) 
        {
            Debug.LogWarning("No assigned actor for camera to follow");
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (FollowActor) 
        {
            transform.position = FollowActor.transform.position + CameraOffset;
        }
    }
}
