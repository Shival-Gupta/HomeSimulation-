using UnityEngine;
using System.Collections.Generic;

public class IoTManager : MonoBehaviour
{
    [Header("IoT Devices (Assign in Inspector)")]
    [Tooltip("Add all IoT device scripts (e.g., LightController) here.")]
    public List<MonoBehaviour> deviceScripts = new List<MonoBehaviour>();

    private Dictionary<string, IIoTDevice> deviceDict = new Dictionary<string, IIoTDevice>();

    private void Awake()
    {
        BuildDeviceDictionary();
    }

    // Call this method to refresh or initially build the device dictionary.
    public void BuildDeviceDictionary()
    {
        deviceDict.Clear();
        foreach (var script in deviceScripts)
        {
            if (script is IIoTDevice device)
            {
                if (!deviceDict.ContainsKey(device.DeviceId))
                {
                    deviceDict.Add(device.DeviceId, device);
                    Debug.Log($"[IoTManager] Added device: {device.DeviceId} at {device.Location}");
                }
                else
                {
                    Debug.LogWarning($"[IoTManager] Duplicate device ID: {device.DeviceId}");
                }
            }
            else
            {
                Debug.LogWarning($"[IoTManager] Script {script.name} does not implement IIoTDevice.");
            }
        }
    }

    // Called by the NetworkManager or via Inspector to process commands.
    public void ProcessDeviceCommand(string deviceId, string command, string parametersJson)
    {
        if (deviceDict.TryGetValue(deviceId, out IIoTDevice device))
        {
            device.ProcessCommand(command, parametersJson);
        }
        else
        {
            Debug.LogWarning($"[IoTManager] Device {deviceId} not found.");
        }
    }
}
