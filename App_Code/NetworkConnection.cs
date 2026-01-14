using System;
using System.Net;
using System.Runtime.InteropServices;

public class NetworkConnection : IDisposable
{
    private readonly string _networkName;

    public NetworkConnection(string networkName, NetworkCredential credentials)
    {
        _networkName = networkName;

        var netResource = new NetResource()
        {
            Scope = 2,
            ResourceType = 1,
            DisplayType = 3,
            Usage = 1,
            RemoteName = networkName
        };

        var userName = string.IsNullOrEmpty(credentials.Domain) ? credentials.UserName : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

        var result = WNetAddConnection2(netResource, credentials.Password, userName, 0);

        if (result != 0)
        {
            throw new InvalidOperationException(string.Format("Error connecting to remote share (Code: {0})", result));
        }
    }

    public void Dispose()
    {
        WNetCancelConnection2(_networkName, 0, true);
    }

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource,
        string password, string username, int flags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags, bool force);

    [StructLayout(LayoutKind.Sequential)]
    private class NetResource
    {
        public int Scope;
        public int ResourceType;
        public int DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }
}
