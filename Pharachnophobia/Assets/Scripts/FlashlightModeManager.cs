using UnityEngine;
using UnityEngine.UI;

public class FlashlightModeManager : MonoBehaviour
{
    public static FlashlightModeManager Instance;

    [Header("UI Flashlight Image")]
    public Image flashlightImage;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color blueColor = Color.blue;

    public bool IsBlueMode { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetNormalMode();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleMode();
        }
    }

    private void ToggleMode()
    {
        if (IsBlueMode)
            SetNormalMode();
        else
            SetBlueMode();
    }

    private void SetNormalMode()
    {
        IsBlueMode = false;

        if (flashlightImage != null)
            flashlightImage.color = normalColor;
    }

    private void SetBlueMode()
    {
        IsBlueMode = true;

        if (flashlightImage != null)
            flashlightImage.color = blueColor;
    }
}