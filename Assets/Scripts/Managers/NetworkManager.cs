using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [Header("Network Settings")]
    public int port = 8080;

    [Header("IoT Manager Reference")]
    public IoTManager iotManager;

    private HttpListener listener;
    private Thread listenerThread;
    private bool isRunning = false;

    // Queue to store commands from the network thread.
    private ConcurrentQueue<Action> commandQueue = new ConcurrentQueue<Action>();

    private void Start()
    {
        StartServer();
    }

    private void Update()
    {
        // Execute queued commands on the main thread.
        while (commandQueue.TryDequeue(out Action action))
        {
            action.Invoke();
        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }

    public void StartServer()
    {
        if (!HttpListener.IsSupported)
        {
            Debug.LogError("HttpListener not supported on this platform.");
            return;
        }

        listener = new HttpListener();
        listener.Prefixes.Add($"http://*:{port}/");
        try
        {
            listener.Start();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[NetworkManager] Failed to start HttpListener: {ex.Message}");
            return;
        }
        isRunning = true;
        listenerThread = new Thread(ListenerLoop);
        listenerThread.Start();
        Debug.Log($"[NetworkManager] Server started on port {port}");
    }

    public void StopServer()
    {
        isRunning = false;
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            listener.Close();
        }
        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Abort();
        }
    }

    private void ListenerLoop()
    {
        while (isRunning)
        {
            try
            {
                // Blocks until a request is received.
                HttpListenerContext context = listener.GetContext();
                ProcessRequest(context);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NetworkManager] Listener exception: {ex.Message}");
            }
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        string responseString = "OK";
        try
        {
            // Expected URL: /control?deviceId=...&command=...&params=...
            if (request.Url.AbsolutePath.Equals("/control", StringComparison.OrdinalIgnoreCase))
            {
                string deviceId = request.QueryString["deviceId"];
                string command = request.QueryString["command"];
                string parameters = request.QueryString["params"];
                if (!string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(command))
                {
                    // Enqueue the command so it executes on the main thread.
                    commandQueue.Enqueue(() =>
                    {
                        if (iotManager != null)
                            iotManager.ProcessDeviceCommand(deviceId, command, parameters);
                        else
                            Debug.LogWarning("[NetworkManager] IoTManager reference is missing.");
                    });
                    responseString = $"Command received: deviceId={deviceId}, command={command}, params={parameters}";
                }
                else
                {
                    responseString = "Missing parameters. Expected deviceId, command, and params.";
                }
            }
            else
            {
                responseString = "Invalid endpoint. Use /control.";
            }
        }
        catch (Exception ex)
        {
            responseString = $"Error processing request: {ex.Message}";
        }

        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        using (var output = response.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }
    }
}
