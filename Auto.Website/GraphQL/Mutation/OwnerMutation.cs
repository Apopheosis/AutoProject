using Auto.Data;
using Auto.Data.Entities;
using Auto.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Auto.Website.GraphQL.Mutations;

public class OwnerMutation : ObjectGraphType
{
    private readonly IAutoDatabase _db;

    public OwnerMutation(IAutoDatabase db)
    {
        _db = db;

        Field<OwnerGraphType>("createOwner")
            .Argument<string>("id")
            .Argument<string>("firstName")
            .Argument<string>("lastName")
            .Argument<string>("email")
            .Resolve(Create);
        Field<OwnerGraphType>("updateOwner")
            .Argument<string>("id")
            .Argument<string>("newFirstName")
            .Argument<string>("newLastName")
            .Argument<string>("newEmail")
            .Resolve(Update);
        
    }

    private Owner Create(IResolveFieldContext<object> context)
    {
        var id = context.GetArgument<string>("id");
        var firstName = context.GetArgument<string>("firstName");
       var lastName = context.GetArgument<string>("lastName");
       var email = context.GetArgument<string>("email");
        var newOwner = new Owner
        {
            Id= id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            VehicleCode = "B433PXT"
        };

        _db.CreateOwner(newOwner);

        return _db.FindOwner(id);
    }

    private Owner Update(IResolveFieldContext<object> context)
    {
        var id = context.GetArgument<string>("id");
        var owner = _db.FindOwner(id);
        owner.FirstName = context.GetArgument<string>("newFirstName");
        owner.LastName = context.GetArgument<string>("newLastName");
        _db.UpdateOwner(owner);
        return _db.FindOwner(id);
    }
}
