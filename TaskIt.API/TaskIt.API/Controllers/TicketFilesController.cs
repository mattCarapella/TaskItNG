using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaskIt.API.DTO;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class TicketFilesController : ControllerBase
{
    private readonly ILogger<TicketFilesController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _context;

    public TicketFilesController(ILogger<TicketFilesController> logger,
                                 IMemoryCache memoryCache,
                                 ApplicationDbContext context)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of TicketFile entities for Ticket with a given Id.
    /// </summary>
    // [HttpGet("{ticketId}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<TicketFile[]>> GetTicketFiles([FromQuery] RequestDTO<FileDTO> input, int ticketId)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// GET request to retrieve a TicketFile entity with a given Id for a Ticket with a given Id.
    /// </summary>
    // [HttpGet("{id}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<TicketFile?>> GetProjectFile(int id)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Create a new TicketFile entity.
    /// </summary>
    // [HttpPost]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketFile?>> CreateTicketFile([FromForm] FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Update a TicketFile entity with given Id.
    /// </summary>
    // [HttpPut]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketFile?>> UpdateTicketFile(FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Delete a TicketFile entity with given Id.
    /// </summary>
    // [HttpDelete("{id}")]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<TicketFile?>> DeleteTicketFile(int id)
    // {
    //     throw new NotImplementedException();
    // }
}