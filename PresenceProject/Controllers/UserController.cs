using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Interfaces;
using PresenceProject.Models;
using System.Data;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Administer")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAsync()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administer")]
    public async Task<ActionResult<UserDto>> Get(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    [Authorize(Roles = "Administer")]
    public async Task<ActionResult<UserDto>> Post([FromBody] UserPostModel userModel)
    {
        var newUser = await _userService.AddAsync( userModel);
        return Ok(newUser);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administer")]
    public async Task<ActionResult<UserDto>> Put(int id, [FromBody] UserDto userDto)
    {
        var updatedUser = await _userService.UpdateAsync(userDto, id);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administer")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _userService.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok();
    }
}

