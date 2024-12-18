using System.Runtime.InteropServices.ComTypes;
using System.Data;
namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnLineUsers = new Dictionary<string, List<string>>();

    public Task<bool> UserConnected(string username, string connectionId)
    {
        bool isOnline = false;
        lock (OnLineUsers)
        {
            if (OnLineUsers.ContainsKey(username))
            {
                OnLineUsers[username].Add(connectionId);
            }
            else
            {
                OnLineUsers.Add(username, new List<string> { connectionId });
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        bool isOffline = false;
        lock (OnLineUsers)
        {
            if (!OnLineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

            OnLineUsers[username].Remove(connectionId);

            if (OnLineUsers[username].Count == 0)
            {
                OnLineUsers.Remove(username);
                isOffline = true;
            }

            return Task.FromResult(isOffline);
        }
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onLineUsers;
        lock (OnLineUsers)
        {
            onLineUsers = OnLineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onLineUsers);
    }

    public static Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string> connectionsIds;
        lock (OnLineUsers)
        {
            connectionsIds = OnLineUsers.GetValueOrDefault(username);
        }

        return Task.FromResult(connectionsIds);
    }

}