using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestAPI.Entities;

namespace RestAPI.Authorization
{
    public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _context;
        private readonly ILogger<CreatedMultipleRestaurantsRequirement> _logger;

        public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext context, ILogger<CreatedMultipleRestaurantsRequirement> logger)
        {
            _context = context;
            _logger = logger;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c=> c.Type == ClaimTypes.NameIdentifier).Value);

           var createdRestaurantsCount = _context
               .Restaurants
               .Count(r => r.CreatedById == userId);

           if (createdRestaurantsCount >= requirement.MinimumRestaurantsCreated)
           {
               context.Succeed(requirement);
           }

           return Task.CompletedTask;
        }
    }
}
