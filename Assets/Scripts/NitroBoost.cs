using UnityEngine;

public class NitroBoost : MonoBehaviour
{
    [SerializeField]
    float BoostValue = 0.3f;

    [SerializeField]
    float DestroySecDelay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    public float GetBoostValue()
    {
        return BoostValue;
    }

    public void SafeDestroy()
    {
        Destroy(gameObject, DestroySecDelay);
    }

    private void Animate() 
    {
        gameObject.transform.Rotate(Vector3.forward, 1);
    }
}
