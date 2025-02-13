using UnityEngine;

public class InductionController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Induction Settings")]
    [Range(0, 3)]
    public int heatLevel = 0;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void SetHeatLevel(int level)
    {
        heatLevel = Mathf.Clamp(level, 0, 3);
        Debug.Log($"[{DeviceId}] Heat level set to {heatLevel}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "setheat":
            case "setheatlevel":
                if (int.TryParse(parametersJson, out int level))
                    SetHeatLevel(level);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
