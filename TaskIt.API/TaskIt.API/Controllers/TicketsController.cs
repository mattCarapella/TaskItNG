using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using TaskIt.API.DTO;
using TaskIt.API.DTO.Ticket;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _context;

    public TicketsController(ILogger<ProjectsController> logger,
                             IMemoryCache memoryCache,
                             ApplicationDbContext context)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of Tickets.
    /// </summary>
    [HttpGet]
    [ResponseCache(CacheProfileName = "Any-60")]
    public async Task<RestDTO<Ticket[]>> GetTickets([FromQuery] RequestDTO<TicketDTO> input)
    {
        Ticket[]? result = null;
        var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";

        // Check if the cache contains an entry for the specified key. If it does, get the data and store it in the result 
        //  variable to be returned. If the key is not found, query the db and store the result in the cache for 30 seconds.
        if (!_memoryCache.TryGetValue<Ticket[]>(cacheKey, out result))
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(input.FilterQuery))
            {
                query = query.Where(p => p.Title.Contains(input.FilterQuery));
            }

            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}")
                         .Skip(input.PageIndex * input.PageSize)
                         .Take(input.PageSize);

            result = await query.ToArrayAsync();
            _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        }

        return new RestDTO<Ticket[]>()
        {
            Data = result!,
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = await _context.Tickets.CountAsync(),
            Links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, "Tickets", new { input.PageIndex, input.PageSize } , Request.Scheme)!, "self", "GET")
            }
        };
    }


    /// <summary>
    /// GET request to retrieve a Ticket entity by its Id.
    /// </summary>
    [HttpGet("{id}")]
    [ResponseCache(CacheProfileName = "Any-60")]
    public async Task<RestDTO<Ticket?>> GetTicket(int id)
    {
        Ticket? result = null;
        var cacheKey = $"GetTicket-{id}";

        if (!_memoryCache.TryGetValue<Ticket>(cacheKey, out result))
        {
            result = await _context.Tickets.Where(p => p.Id == id)
                                           .Include(p => p.Notes)
                                           .FirstOrDefaultAsync();

            if (result is null)
                throw new ArgumentException($"Ticket with Id {id} was not found.");

            _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        }

        return new RestDTO<Ticket?>()
        {
            Data = result,
            PageIndex = 0,
            PageSize = 1,
            RecordCount = result is not null ? 1 : 0,
            Links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, "Tickets", new { id }, Request.Scheme)!, "self", "GET")
            }
        };
    }


    /// <summary>
    /// Create a new Ticket entity.
    /// </summary>
    // [HttpPost]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<Ticket?>> CreateTicket(TicketDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Update an existing Ticket entity.
    /// </summary>
    // [HttpPut]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<Ticket?>> UpdateTicket(TicketDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Delete a Ticket entity with given Id.
    /// </summary>
    // [HttpDelete]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<Ticket?>> DeleteTicket(int id)
    // {
    //     throw new NotImplementedException();
    // }

}