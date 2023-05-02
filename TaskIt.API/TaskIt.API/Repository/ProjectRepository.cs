using Microsoft.EntityFrameworkCore;
using TaskIt.API.Interfaces;
using TaskIt.API.Models;

namespace TaskIt.API.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project> GetProjectAsync(int id)
    {
        return await _context.Projects.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Project[]> GetProjectsAsync()
    {
        return await _context.Projects.ToArrayAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(Project project)
    {
        throw new NotImplementedException();
    }
}