using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;

public class FlashlightScript : MonoBehaviour
{
    public Transform FlashlightTransform;
    public float RotationSpeed = 100f;
    public float MaxRotationAngle = 45f;
    public float MinRotationAngle = -45f;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePos = Input.mousePosition;
            this.gameObject.transform.position = mousePos;
            Debug.Log(mousePos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Input.mousePosition;
            this.gameObject.transform.position = mousePos;
        }
    }
}
