﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController(IMediator mediator): ControllerBase
    {
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var restaurants = await mediator.Send(new GetAllRestaurantsQuerry()); 
            return Ok(restaurants);

        }
        [HttpGet("{id}")]
        public async Task <IActionResult> GetById([FromRoute] int id) 
        {
            var restaurant =  await mediator.Send(new GetRestaurantByIdQuerry(id));
            if (restaurant == null)
                return NotFound();
            return Ok(restaurant);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));
            if (isDeleted)
                return NoContent();
            return NotFound();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
        {
            command.Id = id;
            var isUpdated = await mediator.Send( command);
            if (isUpdated)
                return NoContent();
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant( CreateRestaurantCommand command)
        {
            int id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

    }
}
