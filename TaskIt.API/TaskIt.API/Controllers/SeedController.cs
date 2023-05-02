using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskIt.API.Core.Enums;
using TaskIt.API.Models;

namespace TaskIt.API.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger _logger;

    public SeedController(ApplicationDbContext context,
                          IWebHostEnvironment env,
                          ILogger<SeedController> logger)
    {
        _context = context;
        _env = env;
        _logger = logger;
    }

    [HttpPut]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> TestData()
    {
        if (!_context.Projects.Any())
        {
            var now = DateTime.Now;

            var p1 = new Project()
            {
                Id = 1,
                Name = "Web API Demo",
                Description = "Amet est placerat in egestas erat imperdiet sed euismod. In dictum non consectetur a erat. Condimentum lacinia quis vel eros donec ac odio. Justo donec enim diam vulputate ut pharetra. Id aliquet risus feugiat in ante metus dictum. Id faucibus nisl tincidunt eget nullam non nisi est sit. Ac orci phasellus egestas tellus rutrum tellus. Risus viverra adipiscing at in. Lacus vestibulum sed arcu non odio euismod.",
                Status = Core.Enums.Status.UNASSIGNED,
                Priority = Core.Enums.Priority.MED,
                DateCreated = now,
                LastModified = now
            };
            var p2 = new Project()
            {
                Id = 2,
                Name = "Blazor WASM Demo",
                Description = "Magna fringilla urna porttitor rhoncus dolor purus. Pretium aenean pharetra magna ac placerat vestibulum lectus mauris. Orci eu lobortis elementum nibh tellus molestie. Massa vitae tortor condimentum lacinia quis vel eros donec ac. Platea dictumst quisque sagittis purus sit amet volutpat. Amet massa vitae tortor condimentum lacinia quis. Amet cursus sit amet dictum sit amet.",
                Status = Core.Enums.Status.INPROGRESS,
                Priority = Core.Enums.Priority.HIGH,
                DateCreated = now,
                LastModified = now
            };
            var p3 = new Project()
            {
                Id = 3,
                Name = "Angular UI Demo",
                Description = "Mauris vitae ultricies leo integer malesuada. Ornare quam viverra orci sagittis eu volutpat odio facilisis mauris. Ipsum dolor sit amet consectetur. Egestas dui id ornare arcu odio ut sem. Velit dignissim sodales ut eu sem integer vitae justo eget. Id neque aliquam vestibulum morbi blandit cursus risus at. Pellentesque habitant morbi tristique senectus et netus et malesuada.",
                Status = Core.Enums.Status.INPROGRESS,
                Priority = Core.Enums.Priority.HIGH,
                DateCreated = now,
                LastModified = now
            };
            var projects = new List<Project>() { p1, p2, p3 };
            _context.AddRange(projects);

            var projectNotes = new List<ProjectNote>()
            {
                new ProjectNote()
                {
                    Title = "Test Project Note 1",
                    Content = "Egestas sed sed risus pretium. Diam vulputate ut pharetra sit amet. Interdum velit euismod in pellentesque. Arcu cursus vitae congue mauris rhoncus aenean. Arcu risus quis varius quam quisque id. Porta lorem mollis aliquam ut porttitor leo. ",
                    Links = new List<string>()
                    {
                        "https://www.link1.io",
                        "https://www.link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    ProjectId = p1.Id
                },
                new ProjectNote()
                {
                    Title = "Test Project Note 2",
                    Content = "Velit dignissim sodales ut eu sem integer vitae justo eget. Id neque aliquam vestibulum morbi blandit cursus risus at. Pellentesque habitant morbi tristique senectus et netus et malesuada. Non nisi est sit amet facilisis magna etiam tempor.",
                    Links = new List<string>()
                    {
                        "https://www.google.com",
                        "https://www.stackoverflow.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    ProjectId = p1.Id
                },
                new ProjectNote()
                {
                    Title = "Test Project Note 3",
                    Content = "Odio ut sem nulla pharetra diam. Aliquam sem et tortor consequat id. Justo laoreet sit amet cursus sit amet dictum sit amet. Id faucibus nisl tincidunt eget nullam non nisi est. Id eu nisl nunc mi ipsum faucibus vitae aliquet nec. Enim diam vulputate ut pharetra sit. Eget magna fermentum iaculis eu non diam phasellus vestibulum. Viverra nam libero justo laoreet. Turpis in eu mi bibendum. ",
                    Links = new List<string>()
                    {
                        "https://www.github.com",
                        "https://www.amazon.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    ProjectId = p2.Id
                },
                new ProjectNote()
                {
                    Title = "Test Project Note 4",
                    Content = "Diam phasellus vestibulum lorem sed risus ultricies tristique nulla. Tellus orci ac auctor augue mauris. Enim praesent elementum facilisis leo vel fringilla est ullamcorper. Feugiat sed lectus vestibulum mattis ullamcorper velit sed ullamcorper morbi. Amet volutpat consequat mauris nunc congue nisi vitae suscipit tellus.",
                    Links = new List<string>()
                    {
                        "https://www.testlink.net",
                        "https://www/link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    ProjectId = p3.Id
                }
            };
            _context.ProjectNotes.AddRange(projectNotes);

            using var transaction = _context.Database.BeginTransaction();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Projects ON");
            await _context.SaveChangesAsync();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Projects OFF");
            transaction.Commit();

            var t1 = new Ticket()
            {
                Id = 1,
                Title = "Page Responsiveness Issues",
                Description = "Lectus mauris ultrices eros in cursus turpis massa tincidunt dui. Imperdiet sed euismod nisi porta lorem mollis aliquam ut.",
                Type = Core.Enums.TicketType.BUG,
                Status = Core.Enums.Status.UNASSIGNED,
                Priority = Core.Enums.Priority.HIGH,
                DateCreated = now,
                LastModified = now,
                ProjectId = p1.Id
            };
            var t2 = new Ticket()
            {
                Id = 2,
                Title = "Add Like & Dislike Icons",
                Description = "Augue interdum velit euismod in pellentesque massa placerat duis ultricies. At imperdiet dui accumsan sit amet nulla facilisi morbi tempus. Vitae aliquet nec ullamcorper sit amet. ",
                Type = Core.Enums.TicketType.FEATURE,
                Status = Core.Enums.Status.INPROGRESS,
                Priority = Core.Enums.Priority.CRITICAL,
                DateCreated = now,
                LastModified = now,
                ProjectId = p1.Id
            };
            var t3 = new Ticket()
            {
                Id = 3,
                Title = "Mobile First UI Refactor",
                Description = "Elit scelerisque mauris pellentesque pulvinar pellentesque habitant morbi tristique senectus. Tristique magna sit amet purus gravida quis blandit turpis. Lorem ipsum dolor sit amet consectetur adipiscing.",
                Type = Core.Enums.TicketType.FEATURE,
                Status = Core.Enums.Status.SUBMITTED,
                Priority = Core.Enums.Priority.HIGH,
                DateCreated = now,
                LastModified = now,
                ProjectId = p1.Id
            };
            var t4 = new Ticket()
            {
                Id = 4,
                Title = "Write API Hook Documentation",
                Description = "Platea dictumst quisque sagittis purus sit amet. Dignissim cras tincidunt lobortis feugiat vivamus at augue eget arcu. Nunc id cursus metus aliquam eleifend mi. Molestie at elementum eu facilisis sed odio morbi quis. Tincidunt vitae semper quis lectus nulla at. A arcu cursus vitae congue mauris rhoncus.",
                Type = Core.Enums.TicketType.FEATURE,
                Status = Core.Enums.Status.SUBMITTED,
                Priority = Core.Enums.Priority.CRITICAL,
                DateCreated = now,
                LastModified = now,
                ProjectId = p2.Id
            };
            var t5 = new Ticket()
            {
                Id = 5,
                Title = "Font Inconsistancies",
                Description = "Ipsum faucibus vitae aliquet nec ullamcorper sit amet. Non sodales neque sodales ut etiam sit amet nisl. Dictumst quisque sagittis purus sit amet volutpat consequat mauris nunc. Turpis massa tincidunt dui ut ornare. Elementum tempus egestas sed sed risus pretium quam vulputate dignissim.",
                Type = Core.Enums.TicketType.BUG,
                Status = Core.Enums.Status.UNASSIGNED,
                Priority = Core.Enums.Priority.LOW,
                DateCreated = now,
                LastModified = now,
                ProjectId = p2.Id
            };
            var t6 = new Ticket()
            {
                Id = 6,
                Title = "Error When Navigating to Profile Page",
                Description = "Donec enim diam vulputate ut pharetra. Faucibus a pellentesque sit amet porttitor eget dolor morbi. Sed felis eget velit aliquet. Cursus mattis molestie a iaculis at erat. Ultricies tristique nulla aliquet enim tortor at auctor. Phasellus vestibulum lorem sed risus. Arcu non odio euismod lacinia at quis risus. Viverra orci sagittis eu volutpat odio facilisis mauris sit. Commodo odio aenean sed adipiscing diam donec adipiscing tristique risus.",
                Type = Core.Enums.TicketType.BUG,
                Status = Core.Enums.Status.COMPLETED,
                Priority = Core.Enums.Priority.MED,
                DateCreated = now,
                LastModified = now,
                ProjectId = p2.Id
            };
            var t7 = new Ticket()
            {
                Id = 7,
                Title = "Landing Page Design",
                Description = "Nulla aliquet porttitor lacus luctus accumsan tortor. Urna id volutpat lacus laoreet non curabitur. Auctor eu augue ut lectus arcu bibendum at varius vel. Leo vel fringilla est ullamcorper eget. ",
                Type = Core.Enums.TicketType.FEATURE,
                Status = Core.Enums.Status.UNASSIGNED,
                Priority = Core.Enums.Priority.LOW,
                DateCreated = now,
                LastModified = now,
                ProjectId = p3.Id
            };
            var t8 = new Ticket()
            {
                Id = 8,
                Title = "Design Unit Testing",
                Description = "Aliquet lectus proin nibh nisl condimentum id venenatis a. At erat pellentesque adipiscing commodo elit at imperdiet dui. Amet consectetur adipiscing elit pellentesque habitant morbi tristique senectus et. Cras sed felis eget velit.",
                Type = Core.Enums.TicketType.FEATURE,
                Status = Core.Enums.Status.INPROGRESS,
                Priority = Core.Enums.Priority.HIGH,
                DateCreated = now,
                LastModified = now,
                ProjectId = p3.Id
            };
            var tickets = new List<Ticket>() { t1, t2, t3, t4, t5, t6, t7, t8 };
            _context.AddRange(tickets);

            var ticketNotes = new List<TicketNote>()
            {
                new TicketNote()
                {
                    Title = "Test TicketNote 1",
                    Content = "Egestas sed sed risus pretium. Diam vulputate ut pharetra sit amet. Interdum velit euismod in pellentesque. Arcu cursus vitae congue mauris rhoncus aenean. Arcu risus quis varius quam quisque id. Porta lorem mollis aliquam ut porttitor leo. ",
                    Links = new List<string>()
                    {
                        "https://www.link1.io",
                        "https://www.link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t1.Id
                },
                new TicketNote()
                {
                    Title = "Test TicketNote 2",
                    Content = "Velit dignissim sodales ut eu sem integer vitae justo eget. Id neque aliquam vestibulum morbi blandit cursus risus at. Pellentesque habitant morbi tristique senectus et netus et malesuada. Non nisi est sit amet facilisis magna etiam tempor.",
                    Links = new List<string>()
                    {
                        "https://www.google.com",
                        "https://www.stackoverflow.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t1.Id
                },
                new TicketNote()
                {
                    Title = "Test TicketNote 3",
                    Content = "Odio ut sem nulla pharetra diam. Aliquam sem et tortor consequat id. Justo laoreet sit amet cursus sit amet dictum sit amet. Id faucibus nisl tincidunt eget nullam non nisi est. Id eu nisl nunc mi ipsum faucibus vitae aliquet nec. Enim diam vulputate ut pharetra sit. Eget magna fermentum iaculis eu non diam phasellus vestibulum. Viverra nam libero justo laoreet. Turpis in eu mi bibendum. ",
                    Links = new List<string>()
                    {
                        "https://www.github.com",
                        "https://www.amazon.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t2.Id
                },
                new TicketNote()
                {
                    Title = "Test TicketNote 4",
                    Content = "Risus viverra adipiscing at in. Lacus vestibulum sed arcu non odio euismod. Sit amet justo donec enim diam vulputate. Sit amet mauris commodo quis imperdiet massa tincidunt nunc. Consequat mauris nunc congue nisi vitae suscipit tellus mauris. Felis imperdiet proin fermentum leo vel orci porta.",
                    Links = new List<string>()
                    {
                        "https://www.testlink.net",
                        "https://www/link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t4.Id
                },
                new TicketNote()
                {
                    Title = "Test TicketNote 5",
                    Content = "Sit amet volutpat consequat mauris nunc congue. Amet est placerat in egestas erat imperdiet sed euismod. In dictum non consectetur a erat. Condimentum lacinia quis vel eros donec ac odio. Justo donec enim diam vulputate ut pharetra.",
                    Links = new List<string>()
                    {
                        "https://www.testlink.net",
                        "https://www/link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t4.Id
                },
                new TicketNote()
                {
                    Title = "Test TicketNote 6",
                    Content = "Venenatis tellus in metus vulputate eu scelerisque. Justo laoreet sit amet cursus sit amet dictum sit amet. Pharetra convallis posuere morbi leo urna molestie at elementum. Nullam ac tortor vitae purus faucibus. Vitae semper quis lectus nulla. Lectus arcu bibendum at varius vel pharetra vel. Sit amet dictum sit amet justo donec. Purus sit amet luctus venenatis lectus magna fringilla urna porttitor.",
                    Links = new List<string>()
                    {
                        "https://www.testlink.net",
                        "https://www/link2.com"
                    },
                    DateCreated = now,
                    LastModified = now,
                    TicketId = t6.Id
                }
            };
            _context.TicketNotes.AddRange(ticketNotes);

            using var transaction2 = _context.Database.BeginTransaction();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tickets ON");
            await _context.SaveChangesAsync();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tickets OFF");
            transaction2.Commit();

            return new JsonResult(new
            {
                Projects = _context.Projects.Count(),
                Tickets = _context.Tickets.Count(),
                ProjectNotes = _context.ProjectNotes.Count(),
                TicketNotes = _context.TicketNotes.Count(),
                Message = "Database successfully seeded."
            });
        }

        return new JsonResult(new { Message = "Database must be empty in order to seed data." });
    }

    //[HttpPost]
    //[ResponseCache(NoStore = true)]
    //public async Task<IActionResult> UserData()
    //{
    //    throw new NotImplementedException();
    //}


}