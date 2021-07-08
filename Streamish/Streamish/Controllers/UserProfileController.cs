using Microsoft.AspNetCore.Mvc;
using System;
using Streamish.Repositories;
using Streamish.Models;

namespace Streamish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _profileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _profileRepo = userProfileRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_profileRepo.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var profile = _profileRepo.GetById(id);
            if (profile == null)
            {
                return NotFound();

            }
            return Ok(profile);
        }

        [HttpGet("GetVideosByUserProfileId")]
        public IActionResult GetByUserProfileId(int id)
        {
            var profile = _profileRepo.GetVideosByUserProfileId(id);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
        [HttpPost]
        public IActionResult Post(UserProfile profile)
        {
            _profileRepo.Add(profile);
            return CreatedAtAction("Get", new { id = profile.Id }, profile);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserProfile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }
            _profileRepo.Update(profile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _profileRepo.Delete(id);
            return NoContent();
        }
    }
}