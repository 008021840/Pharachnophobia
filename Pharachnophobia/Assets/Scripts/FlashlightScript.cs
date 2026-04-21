using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FlashlightScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click
        {
            Vector3 mousePos = Input.mousePosition;
            this.gameObject.transform.position = mousePos;
            Debug.Log(mousePos);
        }
    }
}
