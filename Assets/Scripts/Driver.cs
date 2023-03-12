using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Driver : MonoBehaviour
{
    [SerializeField] float SteerSpeed = 170f;
    [SerializeField] float MinSpeed = 20f;
    [SerializeField] float MaxSpeed = 50f;
    [SerializeField] Image NitroSprite;
    [SerializeField] float NitroConsumeAmount = 0.03f;
    [SerializeField] float NitroConsumeRate = 0.2f;
    [SerializeField] float NitroRecoverAmount = 0.02f;
    [SerializeField] float NitroRecoverRate = 1f;
    [SerializeField] float FrictionValue = -0.2f;
    [SerializeField] float FrictionRate = 0.5f;

    [SerializeField] float _currentSpeed;
    private float _currentNitro = 0;
    private bool _isBoosting = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!NitroSprite) 
        {
            Debug.LogWarning("Missing reference to a NitroSprite!");
        }
        _currentSpeed = MinSpeed;
        StartCoroutine(FillNitroBar());
        StartCoroutine(ConsumeNitroBar());
        StartCoroutine(ApplyFriction());
    }

    // Update is called once per frame
    void Update()
    {
        ConsumeNitro();
        float steerAmount = Input.GetAxis("Horizontal") * SteerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * _currentSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -steerAmount);
        transform.Translate(0, moveAmount, 0);
        UpdateNitroBar();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boost"))
        {
            if (other.gameObject && other.gameObject.TryGetComponent<NitroBoost>(out NitroBoost boost))
            {
                _currentNitro += boost.GetBoostValue();
                boost.SafeDestroy();
            }
            else
            {
                Debug.LogWarning("The boost does not have a NitroBoost script associated");
            }
        }
    }

    private void ConsumeNitro() 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            _isBoosting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isBoosting = false;
        }
    }


    private void UpdateNitroBar() 
    {
        if (NitroSprite) 
        {
            NitroSprite.fillAmount = _currentNitro;
        }
    }

    private IEnumerator FillNitroBar()
    {
        while (true)
        {
            if (_currentNitro < 1.0f)
            {
                _currentNitro += NitroRecoverAmount;
            }
            yield return new WaitForSeconds(NitroRecoverRate);
        }
    }

    private IEnumerator ConsumeNitroBar()
    {
        while (true)
        {
            yield return new WaitUntil(() => _isBoosting);
            
            if (_currentNitro >= NitroConsumeAmount)
            {
                _currentNitro -= NitroConsumeAmount;
                _currentSpeed += NitroConsumeAmount * 100;
                if (_currentSpeed > MaxSpeed)
                {
                    _currentSpeed = MaxSpeed;
                }
            }

            yield return new WaitForSeconds(NitroConsumeRate);
        }
    }

    private IEnumerator ApplyFriction()
    {
        while (true)
        {
            if (_currentSpeed > MinSpeed)
            {
                _currentSpeed += FrictionValue;
            }
            if (_currentSpeed < MinSpeed)
            {
                _currentSpeed = MinSpeed;
            }
            yield return new WaitForSeconds(FrictionRate);
        }
    }

    private void OnDestroy() 
    {
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
