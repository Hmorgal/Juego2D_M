using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{

    [SerializeField] float EffectAmount;
    GameObject mainCamera;
    Vector3 lastCamPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        lastCamPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 CameraMovement = mainCamera.transform.position - lastCamPosition;

        transform.position += new Vector3(CameraMovement.x * EffectAmount, CameraMovement.y * EffectAmount, 0);

        lastCamPosition = mainCamera.transform.position; 
    }
}
