using UnityEngine;

public class FanController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Fan Settings")]
    public bool isOn = false;
    public int rpm = 400;

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void Toggle(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] Fan turned {(state ? "ON" : "OFF")}");
    }

    public void SetRPM(int newRPM)
    {
        rpm = newRPM;
        Debug.Log($"[{DeviceId}] RPM set to {rpm}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "setrpm":
                if (int.TryParse(parametersJson, out int newRPM))
                    SetRPM(newRPM);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
