using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Interfaces;
using PresenceProject.Core.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PresenceController : ControllerBase
{
    private readonly IPresenceService _presenceService;
    private readonly IMapper _mapper;

    public PresenceController(IPresenceService presenceService, IMapper mapper)
    {
        _presenceService = presenceService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PresenceDto>>> Get()
    {
        var list = await _presenceService.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PresenceDto>> GetById(int id)
    {
        var dto = await _presenceService.GetByIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }
    [HttpPost]
    [Authorize] // הסרתי את Roles אם עובדים רגילים גם משתמשים בזה
    public async Task<ActionResult<PresenceDto>> Post([FromBody] PresenceCreateModel dto)
    {
        Console.WriteLine("📬 נכנסנו ל-POST /api/Presence");

        var userId = int.Parse(User.FindFirst("id")!.Value);

        // מתוך ה־JWT

        var presence = _mapper.Map<Presence>(dto);
        presence.UserId = userId;

        var createdPresence = await _presenceService.AddPresenceAsync(presence);

        var resultDto = _mapper.Map<PresenceDto>(createdPresence);
        return Ok(resultDto);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] PresenceDto presenceDto)
    {
        var updated = await _presenceService.UpdatePresenceAsync(id, presenceDto);
        if (updated == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePresenceAsync(int id)
    {
        await _presenceService.DeletePresenceAsync(id);
        return Ok();
    }
}
