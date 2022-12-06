
using Auto.Data.Entities;
using GraphQL.Types;

namespace Auto.Website.GraphQL.GraphTypes;

public class OwnerGraphType : ObjectGraphType<Owner>
{
    public OwnerGraphType()
    {
        Name = "owner";
        Field(o => o.Id);
        Field(o => o.FirstName);
        Field(o => o.LastName);
        Field(o => o.Email);
        Field(o => o.Vehicle,nullable:false,typeof(VehicleGraphType));
        
    }
}