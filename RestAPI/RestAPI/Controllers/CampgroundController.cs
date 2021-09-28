using Domain.Models.RequestModels;
using Domain.Models.ResponseModels;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Persistence.Repositories;
using RestAPI.Client;
using RestAPI.Client.Models.ResponseModels;
using RestAPI.Models.RequestModels;
using RestAPI.Models.ResponseModels;
using RestAPI.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("campground")]
    public class CampgroundController : ControllerBase
    {
        private readonly ICampgroundsService _campgroundsService;
        private readonly IUserService _userService;

        public CampgroundController(ICampgroundsService campgroundsService, IUserService userService)
        {
            _campgroundsService = campgroundsService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateCampground(CampgroundCreateRequest campground)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var newCampground = await _campgroundsService.CreateAsync(new CampgroundRequestModel
            {
                CampgroundId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = DateTime.Now
            });

            return newCampground;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<GetCampgroundResponse>> GetAllCampgrounds()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

/*            if (userId is null)
            {
                return NotFound();
            }*/

            var user = await _userService.GetUserAsync(userId.Value);

            var allCampgrounds = (await _campgroundsService.GetAllUserItemsAsync(user.UserId))
                .Select(campground => new GetCampgroundResponse
                {
                    CampgroundId = campground.CampgroundId,
                    UserId = campground.UserId,
                    Name = campground.Name,
                    Price = campground.Price,
                    Description = campground.Description,
                    DateCreated = campground.DateCreated
                });

            return allCampgrounds;
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<int>> DeleteAllCampgrounds()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var entriesDeleted = await _campgroundsService.DeleteAllAsync(user.UserId);

            return entriesDeleted;
        }
    }
}
