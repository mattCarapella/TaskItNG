using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TaskIt.API.DTO;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("/[controller]")]
[ApiController]
public class ProjectFilesController : ControllerBase
{
    private readonly ILogger<ProjectFilesController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly ApplicationDbContext _context;

    public ProjectFilesController(ILogger<ProjectFilesController> logger,
                                 IMemoryCache memoryCache,
                                 ApplicationDbContext context)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of ProjectFile entities for Project with a given Id.
    /// </summary>
    // [HttpGet("{projectId}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<ProjectFile[]>> GetProjectFiles([FromQuery] RequestDTO<FileDTO> input, int projectId)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// GET request to retrieve a ProjectFile entity with a given Id.
    /// </summary>
    // [HttpGet("{id}")]
    // [ResponseCache(CacheProfileName = "Any-60")]
    // public async Task<RestDTO<ProjectFile?>> GetProjectFile(int id)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Create a new ProjectFile entity.
    /// </summary>
    // [HttpPost]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectFile?>> CreateProjectFile([FromForm] FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Update a ProjectFile entity with given Id.
    /// </summary>
    // [HttpPut]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectFile?>> UpdateProjectFile(FileDTO model)
    // {
    //     throw new NotImplementedException();
    // }


    /// <summary>
    /// Delete a ProjectFile entity with given Id.
    /// </summary>
    // [HttpDelete("{id}")]
    // [ResponseCache(NoStore = true)]
    // public async Task<RestDTO<ProjectFile?>> DeleteProjectFile(int id)
    // {
    //     throw new NotImplementedException();
    // }

}