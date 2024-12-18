
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

//The name of the class should have been Photo (singular)
[Table("Photos")]
public class Photos
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public bool IsApproved { get; set; }
    public string PublicId { get; set; }
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}