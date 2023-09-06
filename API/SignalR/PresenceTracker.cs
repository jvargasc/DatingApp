namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnLineUsers = new Dictionary<string, List<string>>();

    public Task UserDisconnected(string username, string connectionId)
    {
        lock (OnLineUsers)
        {
            if (!OnLineUsers.ContainsKey(username)) return Task.CompletedTask;

            OnLineUsers[username].Remove(connectionId);

            if (OnLineUsers[username].Count == 0)
            {
                OnLineUsers.Remove(username);
            }

            return Task.CompletedTask;
        }
    }

}