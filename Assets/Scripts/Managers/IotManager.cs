using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class IoTManager : MonoBehaviour
{
    [Header("Assign IoT Device Scripts (implements IIoTDevice)")]
    public List<MonoBehaviour> deviceScripts = new List<MonoBehaviour>();

    // Internal dictionary for fast lookup.
    private Dictionary<string, IIoTDevice> deviceDict = new Dictionary<string, IIoTDevice>();

    [Header("Simulation Controls (Editor)")]
    public string simulationDeviceId = "";
    public string simulationCommand = "";
    public string simulationParameters = "";

    private void Awake()
    {
        BuildDeviceDictionary();
    }

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

    // This method is called by the custom editor button (or can be called manually) to simulate a command.
    public void SimulateCommand()
    {
        ProcessDeviceCommand(simulationDeviceId, simulationCommand, simulationParameters);
    }
}
