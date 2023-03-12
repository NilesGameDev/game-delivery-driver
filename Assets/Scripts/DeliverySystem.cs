using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    [SerializeField] float DetroyPackageAfterSec = 1f;

    private bool _hasPackage = false;
    private SpriteRenderer _driverSprite;
    private Color _defaultDriverColor;

    void Start() 
    {
        if (!gameObject.TryGetComponent<SpriteRenderer>(out _driverSprite)) 
        {
            Debug.LogWarning("This actor does not have sprite renderer component!");
        }
        else 
        {
            _defaultDriverColor = _driverSprite.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Package") && !_hasPackage) {
            if (other.TryGetComponent<SpriteRenderer>(out SpriteRenderer packageSprite)) 
            {
                _driverSprite.color = packageSprite.color;
            }
            _hasPackage = true;
            Destroy(other.gameObject, DetroyPackageAfterSec);
        } 
        else if (other.CompareTag("Customer") && _hasPackage) 
        {
            _driverSprite.color = _defaultDriverColor;
            _hasPackage = false;
        }
    }
}
