// A common interface for all IoT devices.
public interface IIoTDevice
{
    string DeviceId { get; }
    string Location { get; }
    void ProcessCommand(string command, string parametersJson);
}