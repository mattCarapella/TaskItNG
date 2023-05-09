using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskIt.API.DTO.Account;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApiUser> _userManager;
    private readonly SignInManager<ApiUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private readonly ILogger _logger;

    public AccountsController(ApplicationDbContext context, 
                             UserManager<ApiUser> userManager, 
                             SignInManager<ApiUser> signInManager,
                             IMapper mapper,
                             IConfiguration config,
                             ILogger logger)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _config = config;
        _logger = logger;
    }

    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<ActionResult> Login()
    {
        throw new NotImplementedException();
    }


    [HttpPost]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDTO userForRegistration)
    {
        // Check if model state is valid
        if (userForRegistration is null || !ModelState.IsValid)
        {
            var details = new ValidationProblemDetails(ModelState);
            details.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            details.Status = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(details);
        }

        // TODO: Check if the user with username or email already exists.


        // If everything is ok, register a new user and update the db.
        var newUser = _mapper.Map<ApiUser>(userForRegistration);
        var result = await _userManager.CreateAsync(newUser, userForRegistration.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new RegisterResponseDTO { Errors = errors });
        }

        _logger.LogInformation("User {userName} ({email}) has been created.", newUser.UserName, newUser.Email);
        return StatusCode(201, $"User ‘{newUser.UserName}’ has been created.");
    }

}