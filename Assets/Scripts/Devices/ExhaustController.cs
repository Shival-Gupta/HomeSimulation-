using UnityEngine;

public class ExhaustController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Exhaust Settings")]
    public bool isOn = false;
    [Range(1, 3)]
    public int level = 1;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void Toggle(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] Exhaust turned {(state ? "ON" : "OFF")}");
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Clamp(newLevel, 1, 3);
        Debug.Log($"[{DeviceId}] Level set to {level}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "setlevel":
                if (int.TryParse(parametersJson, out int newLevel))
                    SetLevel(newLevel);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
