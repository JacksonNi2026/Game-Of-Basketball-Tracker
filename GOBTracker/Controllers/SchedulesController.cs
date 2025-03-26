
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
    private readonly GOBTrackerDbContext _context;

    public SchedulesController(GOBTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<Game>>> GetTeamSchedule(int teamId)
    {
        return await _context.Games
            .Where(g => g.Team1ID == teamId || g.Team2ID == teamId)
            .OrderBy(g => g.GameDate)
            .ToListAsync();
    }
}
