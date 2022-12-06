using Newtonsoft.Json;

namespace Auto.Data.Entities;

public class Owner
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    
    public string VehicleCode { get; set; }
    
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; }
}