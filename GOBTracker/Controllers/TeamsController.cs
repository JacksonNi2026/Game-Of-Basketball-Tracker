using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly GOBTrackerDbContext _context;

    public TeamsController(GOBTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
    {
        var teams = await _context.Teams.Include(t => t.Players).ToListAsync();
        var teamDTOs = teams.Select(t => new TeamDTO
        {
            TeamID = t.TeamID,
            TeamName = t.TeamName,
            Coach = t.Coach,
            Players = t.Players.Select(p => p.Name).ToList()
        }).ToList();

        return Ok(teamDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDTO>> GetTeam(int id)
    {
        var team = await _context.Teams
            .Include(t => t.Players) 
            .FirstOrDefaultAsync(t => t.TeamID == id);

        if (team == null)
        {
            return NotFound();
        }

        
        var teamDTO = new TeamDTO
        {
            TeamID = team.TeamID,
            TeamName = team.TeamName,
            Coach = team.Coach,
            Players = team.Players.Select(p => p.Name).ToList() 
        };

        return Ok(teamDTO);
    }



    [HttpPost]
    public async Task<ActionResult<Team>> PostTeam([FromBody] CreateTeamDTO teamDTO)
    {
        if (teamDTO == null || string.IsNullOrEmpty(teamDTO.TeamName))
        {
            return BadRequest("Invalid team data.");
        }

        var team = new Team
        {
            TeamName = teamDTO.TeamName,
            Coach = teamDTO.Coach
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeam), new { id = team.TeamID }, team);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> PutTeam(int id, Team team)
    {
        if (id != team.TeamID) return BadRequest();
        _context.Entry(team).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();
        _context.Teams.Remove(team);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
