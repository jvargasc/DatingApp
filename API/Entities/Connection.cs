using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class Connection
{
    public string ConnectionId { get; } = "";
    public string Username { get; set; }

    public Connection()
    {

    }

    public Connection(string connectionId, string username)
    {
        Username = username;
        ConnectionId = connectionId;
    }
}