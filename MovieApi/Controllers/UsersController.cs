using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.core;
using MovieDatabase.Infrastructure;
using SerilogTimings;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly TokenService tokenService;
        private readonly AppDbContext context;
        private readonly ILogger<UsersController> logger;

        public UsersController(UserManager<ApplicationUser> userManager,TokenService tokenService,AppDbContext context, ILogger<UsersController> logger ) 
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.context = context;
            this.logger = logger;


        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid) {
            return BadRequest(ModelState);
            }

            ApplicationUser new_user = new ApplicationUser()
            {
                UserName = request.Email,
                Email = request.Email,
                Role = MovieDatabase.core.Enums.Role.User
            };


            using (Operation.Time("Creating the user in Database"))
            {
                var result = await userManager.CreateAsync(new_user, request.password!);

                if (result.Succeeded)
                {
                    request.password = "";
                    return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

            }

  

            return BadRequest(ModelState);



        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate(AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                return BadRequest("Bad Credentials");
            }


            var valid_password = await userManager.CheckPasswordAsync(user, request.Password!);
            if (!valid_password)
            {
                return BadRequest("Bad Credentials");
            }

            var user_in_db = context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user_in_db is null)
            {
                return Unauthorized();
            }

            var access_token = tokenService.CreateToken(user_in_db);
            await context.SaveChangesAsync();

            return Ok(new AuthResponse()
            {
                Email = user_in_db.Email,
                Token = access_token,
                Username = user_in_db.UserName

            });
        }





    }
}
