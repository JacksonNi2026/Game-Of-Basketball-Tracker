using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PlayersController : ControllerBase
{
    private readonly GOBTrackerDbContext _context;

    public PlayersController(GOBTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        return await _context.Players.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return NotFound();
        return player;
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByTeam(int teamId)
    {
        return await _context.Players.Where(p => p.TeamID == teamId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Player>> PostPlayer([FromBody] CreatePlayerDTO playerDTO)
    {
        if (playerDTO == null || string.IsNullOrEmpty(playerDTO.Name) || playerDTO.TeamID <= 0)
        {
            return BadRequest("Invalid player data.");
        }

        var team = await _context.Teams.FindAsync(playerDTO.TeamID);
        if (team == null)
        {
            return BadRequest("Invalid TeamID. The specified team does not exist.");
        }

        var player = new Player
        {
            Name = playerDTO.Name,
            Position = playerDTO.Position,
            TeamID = playerDTO.TeamID
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.PlayerID }, player);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlayer(int id, Player player)
    {
        if (id != player.PlayerID) return BadRequest();
        _context.Entry(player).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null) return NotFound();
        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
