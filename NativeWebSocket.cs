using System.Runtime.InteropServices;

public enum SocketState : int
{
    Connecting = 0,
    Open = 1,
    Closing = 2,
    Closed = 3,
}

public static class NativeWebSocket
{
    [DllImport("NativeWebSocket.dll", EntryPoint = "initialize_network", CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitializeNetwork();

    [DllImport("NativeWebSocket.dll", EntryPoint = "finalize_network", CallingConvention = CallingConvention.Cdecl)]
    public static extern void FinalizeNetwork();

    [DllImport("NativeWebSocket.dll", EntryPoint = "set_url", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetUrl([MarshalAs(UnmanagedType.LPStr)] string path);

    [DllImport("NativeWebSocket.dll", EntryPoint = "start", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Start();

    [DllImport("NativeWebSocket.dll", EntryPoint = "send_binary", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SendBinaryRaw(byte[] data, int length);

    public static void SendBinary(byte[] data)
    {
        SendBinaryRaw(data, data.Length);
    }

    [DllImport("NativeWebSocket.dll", EntryPoint = "stop", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Stop();

    [DllImport("NativeWebSocket.dll", EntryPoint = "get_state", CallingConvention = CallingConvention.Cdecl)]
    public static extern SocketState GetState();

    [DllImport("NativeWebSocket.dll", EntryPoint = "has_pending_message", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool HasPendingMessage();

    [DllImport("NativeWebSocket.dll", EntryPoint = "get_pending_message", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetPendingMessage(out int length);

    [DllImport("NativeWebSocket.dll", EntryPoint = "pop_pending_message", CallingConvention = CallingConvention.Cdecl)]
    private static extern void PopPendingMessage();

    public static IEnumerable<byte> GetPendingMessage()
    {
        var message = GetPendingMessage(out var length);
        var bytes = new byte[length];
        Marshal.Copy(message, bytes, 0, length);
        PopPendingMessage();
        return bytes;
    }
}
