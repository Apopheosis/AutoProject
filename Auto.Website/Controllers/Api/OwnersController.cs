using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Auto.Data;
using Auto.Data.Entities;
using Auto.Messages;
using Auto.Website.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace Auto.Website.Controllers.Api;
[Route("api/owners")]
[ApiController]
public class OwnersController : ControllerBase
{
   private readonly IAutoDatabase db;
   private readonly IBus bus;

   public OwnersController(IAutoDatabase db,IBus bus) {
      this.db = db;
      this.bus = bus;
   }
    private dynamic Paginate(string url, int index, int count, int total) {
         dynamic links = new ExpandoObject();
         links.self = new { href = url };
         links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
         links.first = new { href = $"{url}?index=0&count={count}" };
         if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
         if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
         return links;
      }

      
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0,int count = 5)
    {
       var items = db.ListOwners().Skip(index).Take(count);
       var totalItems = db.CountOwners();
       var _links = Paginate("/api/owners/", index, totalItems, count);
       var _actions = new
       {
          create = new
          {
             method = "POST",
             type = "application/json",
             name = "Add a new owner",
             href = "/api/owners"
          },
          delete = new
          {
             method = "DELETE",
             name = "Delete an owner",
             href = "/api/owners/{id}"
          }
       };
       var result = new
       {
          _links,
          _actions,
          total = totalItems,
          index,
          count,
          items
       };

       return Ok(result);
    }

      
      [HttpGet("{id}")]
      public IActionResult Get(string id) {
         var owner = db.FindOwner(id);
         if (owner == default) return NotFound();
         var json = owner.ToDynamic();
         json._links = new {
            self =  $"/api/owners/{owner.Id}",
            vehicle=  $"/api/vehicles/{owner.VehicleCode}" 
            
         };
         json._actions = new {
            put = new {
               method = "PUT",
               href = $"/api/owners/{id}",
               accept = "application/json"
            },
            delete = new {
               method = "DELETE",
               href = $"/api/owners/{id}"
            }
         };
         return Ok(json);
      }

      private void PublishNewOwnerMessage(Owner owner)
      {
         var message = new NewOwnerMessage()
         {
            LastName = owner. LastName,
             FirstName = owner.FirstName,
             Email=owner.Email
         };
         bus.PubSub.Publish(message);
      }
      [HttpPost]
      public IActionResult Post([FromBody] OwnerDTO dto) {
         
         var newOwner = new Owner {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            VehicleCode = dto.VehicleCode
         };
         var ownerId = db.CreateOwner(newOwner);
         if (ownerId == null) return BadRequest();
         PublishNewOwnerMessage(newOwner);
         return Get(ownerId);
      }

      

      
      [HttpPut("{id}")]
      public IActionResult Put(string id, [FromBody] OwnerDTO dto)
      {
         var owner = db.FindOwner(id);
         if (owner == null) return NotFound();
         var updateOwner = new Owner
         {
            Id = id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            VehicleCode = dto.VehicleCode
         };
            
         
         db.UpdateOwner(updateOwner);
         return Get(id);
      }

      
      [HttpDelete("{id}")]
      public IActionResult Delete(string id) {
         
         db.DeleteOwner(id);
         return Ok();
      }
    
    
}
