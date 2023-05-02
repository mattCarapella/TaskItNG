using TaskIt.API.Models;

namespace TaskIt.API.Interfaces;

public interface IProjectRepository
{


    Task<Project[]> GetProjectsAsync();

    Task<Project> GetProjectAsync(int id);

    void Update(Project project);


    Task<bool> SaveAllAsync();

}