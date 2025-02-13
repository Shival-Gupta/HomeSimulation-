using UnityEngine;

public class WashingMachineController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("Washing Machine Settings")]
    public bool isOn = false;
    public string jobMode = "MEDIUM";

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void Toggle(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] Washing Machine turned {(state ? "ON" : "OFF")}");
    }

    public void StartJob(string mode)
    {
        jobMode = mode;
        Debug.Log($"[{DeviceId}] Washing job started in {jobMode} mode");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    Toggle(state);
                break;
            case "startjob":
                StartJob(parametersJson);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
