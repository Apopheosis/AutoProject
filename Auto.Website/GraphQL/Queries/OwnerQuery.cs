using System.Collections.Generic;
using System.Linq;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Queries;

public class OwnerQuery : ObjectGraphType
{
    private IAutoDatabase _db;

    public OwnerQuery(IAutoDatabase db)
    {
        _db = db;

        Field<OwnerGraphType>("ownerById").Argument<int>("id").Resolve(OwnerById);
        Field<ListGraphType<OwnerGraphType>>("owners").Resolve(GetOwners);
        Field<OwnerGraphType>("ownerByFullName").Argument<string>("firstName").Argument<string>("lastName").Resolve(OwnerByFullName);
        Field<OwnerGraphType>("ownerByVehicleCode").Argument<string>("code").Resolve(OwnerByVehicleCode);

        
    }

    private Owner OwnerByVehicleCode(IResolveFieldContext<object> arg)
    {
        var code = arg.GetArgument<string>("code");
        var owner = _db.FindVehicle(code).Owner;
        return owner;

    }

    private Owner OwnerByFullName(IResolveFieldContext<object> arg)
    {
        var firstName = arg.GetArgument<string>("firstName");
        var lastName = arg.GetArgument<string>("lastName");
        var owner = _db.ListOwners().FirstOrDefault(o => o.FirstName.Equals(firstName) && o.LastName.Equals(lastName));
        return owner;

    }

    private IEnumerable<Owner> GetOwners(IResolveFieldContext<object> arg)
    {
        return _db.ListOwners();
    }

    private Owner OwnerById(IResolveFieldContext<object> arg)
    {
        var id = arg.GetArgument<int>("id").ToString();
        return _db.FindOwner(id);

    }
}