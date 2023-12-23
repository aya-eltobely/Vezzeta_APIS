using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VezetaApi.DTO;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;

namespace VezetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> userManager;
        private IConfiguration configuration;
        private readonly IImageService imageService;

        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration _configuration,
            IImageService _imageService)
        {
            userManager = _userManager;
            configuration = _configuration;
            imageService = _imageService;
        }


        [HttpPost("register")] //api/account/register
        //create dto for register data (  عشان مش استخدم identityuser  واجبره يدخل كل البيانات اللي حتي مش محتاحجها)
        public async Task<IActionResult> Register([FromForm] RegisterUserDTO userDTO, IFormFile file)
        {
            // call service and insert image in db and back with imageId in table image
            int imageId = imageService.UploadImage(file);

            if(imageId != 0)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = userDTO.UserName,
                        Email = userDTO.Email,
                        FirstName = userDTO.FirstName,
                        LastName = userDTO.LastName,
                        PhoneNumber = userDTO.Phone,
                        UserImageId = imageId,
                        Gender = userDTO.Gender,
                        Birthdate = userDTO.Birthdate,

                    };
                    IdentityResult res = await userManager.CreateAsync(user, userDTO.Password);
                    //asign role to patient
                    userManager.AddToRoleAsync(user, WebSiteRoles.SitePatient).GetAwaiter().GetResult();

                    //userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role,WebSiteRoles.SitePatient)).GetAwaiter().GetResult();


                    if (res.Succeeded)
                    {
                        return Ok("Account Created");
                    }
                    else
                    {
                        //forloop to show all errors
                        //foreach (var item in res.Errors)
                        //{
                        return BadRequest(res.Errors);
                        //}

                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest("Invalid File");
            }
        }



        [HttpPost("login")] //api/account/login
        public async Task<IActionResult> LogIn(LoginUseDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                //check username
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {
                    //check pass
                    bool res = await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (res)
                    {
                        //(2)
                        var Allclaims = new List<Claim>();
                        Allclaims.Add(new Claim(ClaimTypes.Name, user.UserName)); //custom claim


                        if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteAdmin))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin));
                        }
                        else if (await userManager.IsInRoleAsync(user, WebSiteRoles.SiteDoctor))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteDoctor));
                        }
                        else if (await userManager.IsInRoleAsync(user, WebSiteRoles.SitePatient))
                        {
                            Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SitePatient));
                        }
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin)); //custom claim
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SiteDoctor)); //custom claim
                        //Allclaims.Add(new Claim(ClaimTypes.Role, WebSiteRoles.SitePatient)); //custom claim



                        Allclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id)); //custom claim
                        Allclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); //predifne claims ==> token id

                        //(3)
                        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secretKey"]));
                        SigningCredentials signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //create token (1)
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: configuration["JWT:issuer"], // web api server url
                            audience: configuration["JWT:audiance"], //angular url
                            claims: Allclaims,
                            expires: DateTime.Now.AddDays(2),
                            signingCredentials: signingCredential
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expiration = myToken.ValidTo
                        }
                            );
                    }
                    else
                    {
                        return Unauthorized();

                    }
                }
                else
                {
                    return Unauthorized();
                }

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
