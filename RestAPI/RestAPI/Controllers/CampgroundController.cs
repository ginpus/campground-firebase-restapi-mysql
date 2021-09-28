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
        public async Task<ActionResult<CampgroundResponse>> CreateCampground(CampgroundCreateorUpdateRequest campground)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var newCampground = new CampgroundRequestModel
            {
                CampgroundId = Guid.NewGuid(),
                UserId = user.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = DateTime.Now
            };

            await _campgroundsService.CreateOrEditAsync(newCampground);

            return newCampground.AsDto();
        }

        [HttpGet]
        [Authorize]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<CampgroundResponse>>> GetAllUserCampgrounds()
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var allCampgrounds = (await _campgroundsService.GetAllUserItemsAsync(user.UserId))
                .Select(campground => new CampgroundResponse
                {
                    CampgroundId = campground.CampgroundId,
                    UserId = campground.UserId,
                    Name = campground.Name,
                    Price = campground.Price,
                    Description = campground.Description,
                    DateCreated = campground.DateCreated
                });

            return Ok(allCampgrounds);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CampgroundResponse>>> GetAllCampgrounds()
        {
            var allCampgrounds = (await _campgroundsService.GetAllItemsAsync())
                .Select(campground => new CampgroundResponse
                {
                    CampgroundId = campground.CampgroundId,
                    UserId = campground.UserId,
                    Name = campground.Name,
                    Price = campground.Price,
                    Description = campground.Description,
                    DateCreated = campground.DateCreated
                });

            return Ok(allCampgrounds);
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

        [HttpDelete]
        [Authorize]
        [Route("{campgroundId}")]
        public async Task<ActionResult<int>> DeleteCampground(Guid campgroundId)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var entryDeleted = await _campgroundsService.DeleteAsync(campgroundId, user.UserId);

            return entryDeleted;
        }

        [HttpPut]
        [Authorize]
        [Route("{campgroundId}")]
        public async Task<ActionResult<CampgroundResponse>> EditCampground(Guid campgroundId, CampgroundCreateorUpdateRequest campground)
        {
            var userId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id");

            if (userId is null)
            {
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId.Value);

            var campgroundToUpdate = await _campgroundsService.GetItemByIdAsync(campgroundId, user.UserId);

            if (campgroundToUpdate is null)
            {
                return NotFound($"Campground item with specified id : `{campgroundId}` does not exist");
            }

            campgroundToUpdate.Name = campground.Name;
            campgroundToUpdate.Price = campground.Price;
            campgroundToUpdate.Description = campground.Description;

            await _campgroundsService.CreateOrEditAsync(new CampgroundRequestModel
            {
                CampgroundId = campgroundToUpdate.CampgroundId,
                UserId = campgroundToUpdate.UserId,
                Name = campgroundToUpdate.Name,
                Price = campgroundToUpdate.Price,
                Description = campgroundToUpdate.Description,
                DateCreated = campgroundToUpdate.DateCreated
            });

            return campgroundToUpdate.AsDto();
        }
    }
}
