using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaskIt.API.DTO;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class ProjectNotesController : ControllerBase
{
    private readonly ILogger<ProjectNotesController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _context;

    public ProjectNotesController(ILogger<ProjectNotesController> logger,
                                 IMemoryCache memoryCache,
                                 ApplicationDbContext context)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of ProjectNote entities for Project with a given Id.
    /// </summary>
    // [HttpGet("{projectId}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<ProjectNote[]>> GetProjectNotes([FromQuery] RequestDTO<FileDTO> input, int projectId)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// GET request to retrieve a ProjectNote entity with a given Id.
    /// </summary>
    // [HttpGet("{id}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<ProjectNote?>> GetProjectNote(int id)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Create a new ProjectNote entity.
    /// </summary>
    // [HttpPost]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectNote?>> CreateProjectNote([FromForm] FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Update a ProjectNote entity with given Id.
    /// </summary>
    // [HttpPut]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectNote?>> UpdateProjectNote(FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Delete a ProjectNote entity with given Id.
    /// </summary>
    // [HttpDelete("{id}")]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectNote?>> DeleteProjectNote(int id)
    // {
    //     throw new NotImplementedException();
    // }
}