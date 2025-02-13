using UnityEngine;

public class TVController : MonoBehaviour, IIoTDevice
{
    [Header("Identification")]
    public string deviceId = "";
    public string location = "Unknown";

    [Header("TV Settings")]
    public bool isOn = false;
    public int volume = 40;
    public int channel = 1;
    public string source = "HDMI1";

    public string DeviceId => string.IsNullOrEmpty(deviceId) ? gameObject.name : deviceId;
    public string Location => location;

    public void ToggleTV(bool state)
    {
        isOn = state;
        Debug.Log($"[{DeviceId}] TV turned {(state ? "ON" : "OFF")}");
    }

    public void SetVolume(int newVolume)
    {
        volume = newVolume;
        Debug.Log($"[{DeviceId}] Volume set to {volume}");
    }

    public void SetChannel(int newChannel)
    {
        channel = newChannel;
        Debug.Log($"[{DeviceId}] Channel set to {channel}");
    }

    public void SetSource(string newSource)
    {
        source = newSource;
        Debug.Log($"[{DeviceId}] Source set to {source}");
    }

    public void ProcessCommand(string command, string parametersJson)
    {
        // For simplicity, parametersJson is a plain string or number.
        switch (command.ToLower())
        {
            case "toggle":
                if (bool.TryParse(parametersJson, out bool state))
                    ToggleTV(state);
                break;
            case "setvolume":
                if (int.TryParse(parametersJson, out int vol))
                    SetVolume(vol);
                break;
            case "setchannel":
                if (int.TryParse(parametersJson, out int ch))
                    SetChannel(ch);
                break;
            case "setsource":
                SetSource(parametersJson);
                break;
            default:
                Debug.LogWarning($"[{DeviceId}] Unknown command: {command}");
                break;
        }
    }
}
