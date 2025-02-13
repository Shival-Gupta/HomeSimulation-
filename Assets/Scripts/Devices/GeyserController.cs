using UnityEngine;

public class GeyserController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Geyser Settings")]
    public bool isOn = false;
    public int temperature = 50;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void Toggle(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] Geyser turned {(state ? "ON" : "OFF")}");
    }

    public void SetTemp(int temp)
    {
        temperature = temp;
        Debug.Log($"[{DeviceId}] Temperature set to {temperature}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "settemp":
                if (int.TryParse(parametersJson, out int temp))
                    SetTemp(temp);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
