using UnityEngine;

public class ACController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("AC Settings")]
    public bool isOn = false;
    [Range(16, 30)]
    public int temperature = 24;
    public int fanSpeed = 1;
    public bool ecoMode = false;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void Toggle(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] AC turned {(state ? "ON" : "OFF")}");
    }

    public void SetTemperature(int newTemp)
    {
        temperature = Mathf.Clamp(newTemp, 16, 30);
        Debug.Log($"[{DeviceId}] Temperature set to {temperature}");
    }

    public void SetFanSpeed(int speed)
    {
        fanSpeed = speed;
        Debug.Log($"[{DeviceId}] Fan speed set to {fanSpeed}");
    }

    public void ToggleEcoMode()
    {
        ecoMode = !ecoMode;
        Debug.Log($"[{DeviceId}] Eco Mode toggled {(ecoMode ? "ON" : "OFF")}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "settemperature":
                if (int.TryParse(parametersJson, out int temp))
                    SetTemperature(temp);
                break;
            case "setfanspeed":
                if (int.TryParse(parametersJson, out int speed))
                    SetFanSpeed(speed);
                break;
            case "toggleeco":
                ToggleEcoMode();
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
