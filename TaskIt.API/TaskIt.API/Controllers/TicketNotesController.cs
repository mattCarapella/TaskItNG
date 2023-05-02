using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaskIt.API.DTO;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class TicketNotesController : ControllerBase
{
    private readonly ILogger<TicketNotesController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _context;

    public TicketNotesController(ILogger<TicketNotesController> logger,
                                 IMemoryCache memoryCache,
                                 ApplicationDbContext context)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of TicketNote entities for Ticket with a given Id.
    /// </summary>
    // [HttpGet("{ticketId}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<TicketNote[]>> GetTicketFiles([FromQuery] RequestDTO<FileDTO> input, int ticketId)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// GET request to retrieve a TicketNote entity with a given Id for a Ticket with a given Id.
    /// </summary>
    // [HttpGet("{id}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<TicketNote?>> GetProjectFile(int id)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Create a new TicketNote entity.
    /// </summary>
    // [HttpPost]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketNote?>> CreateTicketNote([FromForm] FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Update a TicketNote entity with given Id.
    /// </summary>
    // [HttpPut]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketNote?>> UpdateTicketNote(FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Delete a TicketNote entity with given Id.
    /// </summary>
    // [HttpDelete("{id}")]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketNote?>> DeleteTicketNote(int id)
    // {
    //     throw new NotImplementedException();
    // }
}