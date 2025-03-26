using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly GOBTrackerDbContext _context;

    public StatsController(GOBTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stat>>> GetStats()
    {
        return await _context.Stats.ToListAsync();
    }

    [HttpGet("player/{playerId}")]
    public async Task<ActionResult<IEnumerable<Stat>>> GetStatsByPlayer(int playerId)
    {
        return await _context.Stats.Where(s => s.PlayerID == playerId).ToListAsync();
    }

    [HttpGet("game/{gameId}")]
    public async Task<ActionResult<IEnumerable<Stat>>> GetStatsByGame(int gameId)
    {
        return await _context.Stats.Where(s => s.GameID == gameId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Stat>> PostStat(Stat stat)
    {
        _context.Stats.Add(stat);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStatsByPlayer), new { playerId = stat.PlayerID }, stat);
    }
}
