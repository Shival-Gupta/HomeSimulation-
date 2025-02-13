using UnityEngine;

public class LightController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Light Settings")]
    public bool isOn = false;
    [Range(0f, 2f)]
    public float intensity = 1.0f;
    [Tooltip("Hex color (e.g., #FFFFFF or FFFFFF)")]
    public string hexColor = "#FFFFFF";

    private Light lightSource;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
        if (lightSource == null)
        {
            Debug.LogWarning($"[{DeviceId}] Missing Light component. This is a simulation.");
        }
    }

    public void Toggle(bool state)
    {
        isOn = state;
        if (lightSource != null)
            lightSource.enabled = state;
        Debug.Log($"[{DeviceId}] Toggled {(state ? "ON" : "OFF")}");
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = Mathf.Clamp(newIntensity, 0f, 2f);
        if (lightSource != null)
            lightSource.intensity = intensity;
        Debug.Log($"[{DeviceId}] Intensity set to {intensity}");
    }

    public void SetHue(string newHex)
    {
        string formattedHex = newHex.StartsWith("#") ? newHex : "#" + newHex;
        if (ColorUtility.TryParseHtmlString(formattedHex, out Color color))
        {
            hexColor = formattedHex;
            if (lightSource != null)
                lightSource.color = color;
            Debug.Log($"[{DeviceId}] Color set to {hexColor}");
        }
        else
        {
            Debug.LogError($"[{DeviceId}] Invalid hex color: {newHex}");
        }
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "setintensity":
                if (float.TryParse(parametersJson, out float newIntensity))
                    SetIntensity(newIntensity);
                break;
            case "sethue":
                SetHue(parametersJson);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
