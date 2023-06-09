﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using TaskIt.API.DTO;
using TaskIt.API.DTO.Project;
using TaskIt.API.Models;
using TaskIt.API.Services;

namespace TaskIt.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ProjectsController> _logger;
    private readonly ErrorService _errorService;
    private readonly ApplicationDbContext _context;

    public ProjectsController(IMapper mapper,
                              IMemoryCache memoryCache,
                              ILogger<ProjectsController> logger,
                              ErrorService errorService,
                              ApplicationDbContext context)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _logger = logger;
        _errorService = errorService;
        _context = context;
    }

    /// <summary>
    /// GET request to retrieve a list of projects.
    /// </summary>
    [HttpGet]
    [ResponseCache(CacheProfileName = "Any-60")]
    public async Task<IActionResult> GetProjects([FromQuery] RequestDTO<ProjectListResultDTO> input)
    {
        IEnumerable<ProjectListResultDTO>? result = null;
        int recordCount = 0;
        var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";

        // Check if the cache contains an entry for the specified key and use the cached data if it does.
        if (!_memoryCache.TryGetValue<IEnumerable<ProjectListResultDTO>>(cacheKey, out result))
        {
            // If the key is not found, query the db and store the result in the cache for 30 seconds.
            var query = _context.Projects.AsQueryable();

            // Filter and paginate the result.
            if (!string.IsNullOrEmpty(input.FilterQuery))
                query = query.Where(p => p.Name.Contains(input.FilterQuery));
            if (input.Status.HasValue)
                query = query.Where(p => p.Status == input.Status);
            if (input.Priority.HasValue)
                query = query.Where(p => p.Priority == input.Priority);

            query = query.OrderBy($"{input.SortColumn} {input.SortOrder}")
                            .Skip(input.PageIndex * input.PageSize)
                            .Take(input.PageSize);

            recordCount = await query.CountAsync();

            result = await (from p in query
                            select new ProjectListResultDTO()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description,
                                Archived = p.Archived,
                                Flagged = p.Flagged,
                                Status = p.Status,
                                Priority = p.Priority,
                                GoalDate = p.GoalDate,
                                DateClosed = p.DateClosed,
                                DateCreated = p.DateCreated,
                                TicketCount = p.Tickets!.Count()
                            }).ToListAsync();

            _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 10));
        }

        var response = new ApiResultDTO<IEnumerable<ProjectListResultDTO>>()
        {
            Data = result!,
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            TotalPages = (int)Math.Ceiling(recordCount / (double)input.PageSize),
            RecordCount = recordCount,
            Links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, "Projects", new { input.PageIndex, input.PageSize } , Request.Scheme)!, "self", "GET")
            }
        };

        return Ok(response);
    }


    /// <summary>
    /// GET request to retrieve a Project by given Id.
    /// </summary>
    [HttpGet("{id}")]
    [ResponseCache(CacheProfileName = "Any-60")]
    public async Task<IActionResult> GetProject(int id)
    {
        ProjectDTO? result = null;
        var cacheKey = $"GetProject-{id}";

        // Check if the cache contains an entry for the specified key and use the cached data if it does.
        if (!_memoryCache.TryGetValue<ProjectDTO>(cacheKey, out result))
        {
            // If the key is not found, query the db and store the result in the cache for 30 seconds.
            var project = await _context.Projects.Where(p => p.Id == id)
                                                .Include(p => p.Tickets)
                                                .Include(p => p.Notes)
                                                .FirstOrDefaultAsync();

            if (project is null)
            {
                _errorService.LogError("Could not process request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
                _errorService.LogError($"Item with ID {id} was not found.");
                return _errorService.NotFoundProblemDetails(id);
            }

            result = new ProjectDTO()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Archived = project.Archived,
                Flagged = project.Flagged,
                Status = project.Status,
                Priority = project.Priority,
                GoalDate = project.GoalDate,
                DateClosed = project.DateClosed,
                DateCreated = project.DateCreated,
                Tickets = project.Tickets!.Select(t => new ProjectTicketsDTO()
                {
                    Id = t.Id,
                    Title = t.Title,
                    DateCreated = t.DateCreated,
                    GoalDate = t.GoalDate,
                    Priority = t.Priority
                }),
                Notes = project.Notes!.Select(n => new ProjectNotesDTO()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content
                })
            };

            _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        }

        var response = new ApiResultDTO<ProjectDTO?>()
        {
            Data = result,
            PageIndex = 0,
            PageSize = 1,
            TotalPages = 1,
            RecordCount = result is not null ? 1 : 0,
            Links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, "Projects", new { id }, Request.Scheme)!, "self", "GET")
            }
        };

        return Ok(response);
    }


    /// <summary>
    /// Create a new Project entity.
    /// </summary>
    [HttpPost]
    [Route("/[action]")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> Create([FromBody] ProjectCreateDTO model)
    {
        if (model is null || !ModelState.IsValid)
        {
            _errorService.LogError("Could not proceess request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            _errorService.LogError("Invalid model state.");
            return _errorService.InvalidModelProblemDetails();
        }

        // Check if a project with the same name already exists.
        var proj = await _context.Projects.Where(p => p.Name.Trim().ToLower() == model!.Name.TrimEnd().ToLower())
                                          .FirstOrDefaultAsync();

        if (proj is not null)
        {
            _errorService.LogError("Could not proceess request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            _errorService.LogError($"Project with name \"{model.Name}\" already exists.");
            return _errorService.InvalidNameProblemDetails(model.Name);
        }

        // If it doesn't, create a new project and update the db.
        var newProject = new Project();
        newProject = _mapper.Map<Project>(model);
        newProject.Archived = false;
        newProject.Flagged = false;
        newProject.Status = Core.Enums.Status.UNASSIGNED;
        newProject.DateCreated = DateTime.Now;
        newProject.LastModified = DateTime.Now;

        _context.Projects.Add(newProject);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
            throw new InvalidOperationException("Something went wrong when attempting to create this project.");

        var newProjectDTO = _mapper.Map<ProjectDTO>(newProject);

        var response = new ApiResultDTO<ProjectDTO?>()
        {
            Data = newProjectDTO,
            Links = new List<LinkDTO>()
                {
                    new LinkDTO(Url.Action(null, "Projects", model, Request.Scheme)!, "self", "POST")
                },
            Message = "Project successfully created."
        };

        return Ok(response);
    }


    /// <summary>
    /// Update a Project entity.
    /// </summary>
    [HttpPut]
    [Route("/[action]")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> Update([FromBody] ProjectUpdateDTO model)
    {
        if (model is null || !ModelState.IsValid)
        {
            _errorService.LogError("Could not proceess request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            _errorService.LogError("Invalid model state.");
            return _errorService.InvalidModelProblemDetails();
        }

        // Make sure the project being updated exists.
        var project = await _context.Projects.Where(p => p.Id == model!.Id).FirstOrDefaultAsync();

        if (project is null)
        {
            _errorService.LogError("Could not process request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            _errorService.LogError($"Item with ID {model.Id} was not found.");
            return _errorService.NotFoundProblemDetails(model.Id);
        }

        // If it does, update the properties and save the changes.
        if (!string.IsNullOrEmpty(model!.Name))
            project.Name = model.Name;
        if (!string.IsNullOrEmpty(model.Description))
            project.Description = model.Description;
        if (model.Archived.HasValue)
            project.Archived = model.Archived.Value;
        if (model.Flagged.HasValue)
            project.Flagged = model.Flagged.Value;
        if (model.Status.HasValue)
            project.Status = model.Status.Value;
        if (model.Priority.HasValue)
            project.Priority = model.Priority.Value;
        if (model.GoalDate.HasValue)
            project.GoalDate = model.GoalDate.Value;
        if (model.DateClosed.HasValue)
            project!.DateClosed = model.DateClosed.Value;
        project!.LastModified = DateTime.Now;

        _context.Projects.Update(project);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Something went wrong when attempting to update this project.");
        }

        var projectDTO = _mapper.Map<ProjectDTO>(project);

        var response = new ApiResultDTO<ProjectDTO?>()
        {
            Data = projectDTO,
            Links = new List<LinkDTO>()
            {
                new LinkDTO(Url.Action(null, "Projects", model, Request.Scheme)!, "self", "PUT")
            },
            Message = "Project successfully updated."
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a Project entity with given Id.
    /// </summary>
    [HttpDelete]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        // Make sure the project to be deleted exists.
        var project = await _context.Projects.Where(p => p.Id == id).FirstOrDefaultAsync();
        
        if (project is null)
        {
            _errorService.LogError("Could not process request on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.Id);
            _errorService.LogError($"Item with ID {id} was not found.");
            return _errorService.NotFoundProblemDetails(id);
        }

        // If it does, delete it and update the db.
        _context.Projects.Remove(project);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Something went wrong when attempting to delete this project.");
        }

        var response = new ApiResultDTO<Project?>()
        {
            Data = project,
            Links = new List<LinkDTO>
            {
                new LinkDTO(Url.Action(null, "Projects", id, Request.Scheme)!, "self", "DELETE")
            },
            Message = "Project successfully deleted."
        };

        return Ok(response);
    }
}













//public async Task<RestDTO<ProjectDTO[]>> GetProjects([FromQuery] RequestDTO<ProjectDTO> input)
//{
//    ProjectDTO[]? result = null;
//    int recordCount = 0;
//    var cacheKey = $"{input.GetType()}-{JsonSerializer.Serialize(input)}";

//    // Check if the cache contains an entry for the specified key. If it does, get the data and store it in the result 
//    //  variable to be returned. If the key is not found, query the db and store the result in the cache for 30 seconds.
//    if (!_memoryCache.TryGetValue<ProjectDTO[]>(cacheKey, out result))
//    {
//        var query = _context.Projects.AsQueryable();

//        // Filter by search term
//        if (!string.IsNullOrEmpty(input.FilterQuery))
//            query = query.Where(p => p.Name.Contains(input.FilterQuery));

//        // Filter by Status and Priority
//        if (input.Status.HasValue)
//            query = query.Where(p => p.Status == input.Status);
//        if (input.Priority.HasValue)
//            query = query.Where(p => p.Priority == input.Priority);

//        // Pagination
//        query = query.OrderBy($"{input.SortColumn} {input.SortOrder}")
//                     .Skip(input.PageIndex * input.PageSize)
//                     .Take(input.PageSize);

//        recordCount = await query.CountAsync();

//        result = await (from p in query
//                        select new ProjectDTO()
//                        {
//                            Id = p.Id,
//                            Name = p.Name,
//                            Description = p.Description,
//                            Archived = p.Archived,
//                            Flagged = p.Flagged,
//                            Status = p.Status,
//                            Priority = p.Priority,
//                            GoalDate = p.GoalDate,
//                            DateClosed = p.DateClosed,
//                            DateCreated = p.DateCreated,
//                            TicketCount = p.Tickets!.Count()
//                        }).ToArrayAsync();

//        // Cache the result
//        _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 10));
//    }

//    return new RestDTO<ProjectDTO[]>()
//    {
//        Data = result!,
//        PageIndex = input.PageIndex,
//        PageSize = input.PageSize,
//        RecordCount = recordCount,
//        Links = new List<LinkDTO>
//        {
//                new LinkDTO(Url.Action(null, "Projects", new { input.PageIndex, input.PageSize } , Request.Scheme)!, "self", "GET")
//            }
//    };
//}





// From project create
//
//public async Task<IActionResult> Update([FromBody] ProjectUpdateDTO model)
//{
//    if (!ModelState.IsValid) return BadRequest(ModelState);

//    // Make sure the project being updated exists.
//    var projectToUpdate = await _context.Projects.Where(p => p.Id == model.Id).FirstOrDefaultAsync();
//    if (projectToUpdate is null)
//    {
//        ModelState.AddModelError("", $"Project with Id {model.Id} was not found.");
//        return StatusCode(404, ModelState);
//    }

//    // If it does, update the properties and save the changes.
//    projectToUpdate = _mapper.Map<Project>(model);
//    projectToUpdate.LastModified = DateTime.Now;

//    _context.Projects.Update(projectToUpdate);
//    var result = await _context.SaveChangesAsync() > 0;

//    if (!result)
//    {
//        ModelState.AddModelError("", "Something went wrong when attempting to update this project.");
//        return StatusCode(500, ModelState);
//    }

//    var projectDTO = _mapper.Map<ProjectDTO>(projectToUpdate);

//    var response = new ApiResultDTO<ProjectDTO?>()
//    {
//        Data = projectDTO,
//        Links = new List<LinkDTO>()
//        {
//                new LinkDTO(Url.Action(null, "Projects", model, Request.Scheme)!, "self", "PUT")
//            },
//        Message = "Project successfully updated."
//    };

//    return Ok(response);
//}




// From GET Project

// If the key is not found, query the db and store the result in the cache for 30 seconds.
//result = await _context.Projects
//                       .Select(p => new ProjectDTO()
//                       {
//                           Id = p.Id,
//                           Name = p.Name,
//                           Description = p.Description,
//                           Archived = p.Archived,
//                           Flagged = p.Flagged,
//                           Status = p.Status,
//                           Priority = p.Priority,
//                           GoalDate = p.GoalDate,
//                           DateClosed = p.DateClosed,
//                           DateCreated = p.DateCreated,
//                           Tickets = p.Tickets!.Select(t => new ProjectTicketsDTO()
//                           {
//                               Id = t.Id,
//                               Title = t.Title,
//                               DateCreated = t.DateCreated,
//                               GoalDate = t.GoalDate,
//                               Priority = t.Priority
//                           }),
//                           Notes = p.Notes!.Select(n => new ProjectNotesDTO()
//                           {
//                               Id = n.Id,
//                               Title = n.Title,
//                               Content = n.Content
//                           })
//                       }).SingleOrDefaultAsync(p => p.Id == id);