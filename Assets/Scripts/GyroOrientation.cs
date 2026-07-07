using UnityEngine;

public class GyroOrientation : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;

    RectTransform rectTransform;
    public float compassSmooth = 0.5f;
    private float m_lastMagneticHeading = 0f;
    private void Start()
    {
        Input.gyro.enabled = true;
        rectTransform = GetComponent<RectTransform>();
        Input.location.Start();
        // Start the compass.
        Input.compass.enabled = true;
    }

    void Update()
    {
        if (isPlayer)
        {
            RotateUsingGyroRotationRateUnbiased();
        }
    }


    void RotateUsingGyroRotationRateUnbiased()
    {
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y - Input.gyro.rotationRateUnbiased.z * Time.deltaTime * Mathf.Rad2Deg, 0.0f);
    }
}
