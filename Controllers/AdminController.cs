using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using VezetaApi.DTO;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;
using static System.Net.Mime.MediaTypeNames;
using Image = VezetaApi.Models.Image;

namespace VezetaApi.Controllers
{
    /// /////////////////////////////////////  Admin ////////////////////////////////////////////// 

    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = WebSiteRoles.SiteAdmin)]
    public class AdminController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        public ApplicationDbContext Context;
        private readonly IConfiguration configuration;
        private readonly IImageService imageService;
        private readonly IEmailService emailService;

        public AdminController(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> _userManager,
            ApplicationDbContext _Context, IConfiguration _configuration,
            IImageService _imageService , IEmailService _emailService)
        {
            unitOfWork = _unitOfWork;
            userManager = _userManager;
            Context = _Context;
            configuration = _configuration;
            imageService = _imageService;
            emailService = _emailService;
        }


        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //var user = await userManager.FindByIdAsync(userId);
        //var roles = await userManager.GetRolesAsync(user);
        //var x = await userManager.IsInRoleAsync(user, "Admin");
        //var y = roles.ToList();
        /// ///////////////////////////Statistic/////////////////////////////////

        //NumOfDoctors 
        [HttpGet("NumOfDoctors")]
        public IActionResult NumOfDoctors()
        {
            var num =  unitOfWork.Doctor.GetAll().Count();
            if(num> 0)
            {
                return Ok(num);
            }
            else
            return Ok(0); 
        }

        //NumOfPations 

        [HttpGet("NumOfPatients")]
        public async Task<IActionResult> NumOfPatients()
        {
            //var users = unitOfWork.AppUser.GetAll().ToList();
            var Patients = await userManager.GetUsersInRoleAsync(WebSiteRoles.SitePatient);
            var num = Patients.Count();
            if (num > 0)
            {
                return Ok(num);
            }
            else
                return Ok(0);
        }

        //NumOfRequests

        [HttpGet("NumOfRequests")]
        public IActionResult NumOfRequests()
        {

            var AllRequests = unitOfWork.Booking.GetAll();
            var penddingReq = AllRequests.Where(r => r.RequestStatus == RequestStatus.PendingRequests).ToList().Count();
            var completedReq = AllRequests.Where(r => r.RequestStatus == RequestStatus.completedRequests).ToList().Count();
            var CancelledReq = AllRequests.Where(r => r.RequestStatus == RequestStatus.CancelledRequests).ToList().Count();

            RequestDTO Requests = new RequestDTO()
            {
                Requests = AllRequests.Count(),
                PendingRequests = penddingReq,
                completedRequests = completedReq,
                CancelledRequests = CancelledReq
            };
            return Ok(Requests);
        }


        //Top 5 Specializations
        [HttpGet("TopFiveSpecializations")]
        public IActionResult TopFiveSpecializations()
        {
            var TopSpecializations = (from b in Context.Bookings
                                      join s in Context.Specializations on b.SpecializationId equals s.Id
                                      group b by b.SpecializationId into g
                                      orderby g.Count() descending
                                      select new
                                      {
                                          SpecializationId = g.Key,
                                          BookingCount = g.Count()
                                      }).Take(5);
            return Ok(TopSpecializations);
        }


        //Top 10 Doctors
        [HttpGet("TopTenDoctors")]
        public IActionResult TopTenDoctors()
        {
            var TopDoctors = (from b in Context.Bookings
                              join d in Context.Doctors on b.DoctorId equals d.Id
                              join u in Context.ApplicationUsers on d.userId equals u.Id
                              join s in Context.Specializations on d.SpecializationId equals s.Id
                              group b by new { u.UserImageId, FullName = u.FirstName + " " + u.LastName, s.Name } into g
                              orderby g.Count() descending
                              select new
                              {
                                  g.Key.UserImageId,
                                  FullName = g.Key.FullName,
                                  SpecializationName = g.Key.Name,
                                  BookingCount = g.Count()
                              }).ToList();

            return Ok(TopDoctors);
        }



        //////////////////////////////////////////// Doctors ////////////////////////
        ///

        [HttpPost("Doctor")]
        public async Task<IActionResult> AddDoctor( [FromForm]AddDoctorDTO DoctorDTO ,  IFormFile file)
        {
            // call service and insert image in db and back with imageId in table image
            int imageId = imageService.UploadImage(file) ;

            if(imageId != 0)
            {
                if (ModelState.IsValid)
                {

                    ApplicationUser user = new ApplicationUser()
                    {
                        FirstName = DoctorDTO.FirstName,
                        LastName = DoctorDTO.LastName,
                        Email = DoctorDTO.Email,
                        PhoneNumber = DoctorDTO.Phone,
                        Gender = DoctorDTO.Gender,
                        Birthdate = DoctorDTO.Birthdate,
                        UserImageId = imageId,
                        UserName = DoctorDTO.UserName
                    };
                    IdentityResult res = await userManager.CreateAsync(user, DoctorDTO.Password);

                    //asign role to patient
                    userManager.AddToRoleAsync(user, WebSiteRoles.SiteDoctor).GetAwaiter().GetResult();
                    //userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, WebSiteRoles.SiteDoctor)).GetAwaiter().GetResult();

                    if (res.Succeeded)
                    {
                        Doctor doctor = new Doctor()
                        {
                            userId = user.Id,
                            SpecializationId = DoctorDTO.SpecializationId
                        };

                        Doctor newDoctor = unitOfWork.Doctor.Create(doctor);
                        await Context.SaveChangesAsync();

                        if (newDoctor != null)
                        {
                            //send mail to doctor with username and password ..............

                             emailService.SendWelcomeEmail(user.Email, user.UserName, DoctorDTO.Password);

                            return Ok(true);

                        }
                        else
                        {
                            return BadRequest();
                        }

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



        [HttpPut("Doctor/{id}")]
        public async Task<IActionResult> EditDoctor(int id , EditDoctorDTO DoctorDTO)
        {
            if(ModelState.IsValid)
            {
                Doctor doctor = unitOfWork.Doctor.GetById(id);
                unitOfWork.Save();
                if (doctor!=null)
                {
                    doctor.SpecializationId = DoctorDTO.SpecializationId;

                    ApplicationUser user = await userManager.FindByIdAsync(doctor.userId);

                    user.UserImageId = DoctorDTO.ImageId;
                    user.FirstName = DoctorDTO.FirstName;
                    user.LastName = DoctorDTO.LastName;
                    user.Email = DoctorDTO.Email;
                    user.PhoneNumber = DoctorDTO.Phone;
                    user.Gender = DoctorDTO.Gender;
                    user.Birthdate = DoctorDTO.Birthdate;

                    unitOfWork.Save();
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Doctor not Exist");
                }

                
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpDelete("Doctor")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            Doctor doctor = unitOfWork.Doctor.GetById(id);
            if (doctor != null)
            {
                var DocBookingCount = unitOfWork.Booking.GetAll(b => b.DoctorId == doctor.Id).ToList().Count();

                if(DocBookingCount > 0)
                {
                    return BadRequest("Cant Delete This Doctor");
                }
                else
                {
                    bool res = unitOfWork.Doctor.Delete(doctor);
                    if (res)
                    {
                        var user = await userManager.FindByIdAsync(doctor.userId);
                        if (user != null)
                        {
                            var res2 = unitOfWork.AppUser.Delete(user);
                            if (res2)
                            {
                                return Ok(true);
                            }
                            else
                            {
                                return BadRequest("Something Wrong");

                            }
                        }
                        else
                        {
                            return BadRequest("user not Exist");
                        }
                    }
                    else
                    {
                        return BadRequest("Something Wrong");
                    }
                }

                
            }
            else
            {
                return BadRequest("Doctor not Exist");
            }
        }



        [HttpGet("Doctor/{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            Doctor doctor = unitOfWork.Doctor.GetById(id, "Specialization,user");
            List<Feedback> DocFeedbacks = unitOfWork.Feedback.GetAll(f => f.DoctorId == doctor.Id, null, "AppUser").ToList(); //(f => f.DoctorId == doctor.Id).ToList();

            // doctor details
            GetDoctorDTO doctorDTO = new GetDoctorDTO()
            {
                ImageId = (int)doctor.user.UserImageId,
                FullName = doctor.user.FirstName + " " + doctor.user.LastName,
                Email = doctor.user.Email,
                Phone = doctor.user.PhoneNumber,
                SpecializeName = doctor.Specialization.Name,
                Gender = doctor.user.Gender
            };

            //feedback
            List<GetFeedbackDTO> feedbackDTOs = new List<GetFeedbackDTO>();
            foreach (var feedback in DocFeedbacks)
            {
                feedbackDTOs.Add(
                new GetFeedbackDTO()
                {
                    ImageId = (int)feedback.AppUser.UserImageId,
                    FullName = feedback.AppUser.FirstName + " " + feedback.AppUser.LastName,
                    FeedbackContent = feedback.Content,
                });
            }

            if (doctor != null)
            {
                return Ok( new {
                    details = doctorDTO,
                    feedbacks = feedbackDTOs
                }); 
            }
            else
            {
                return BadRequest("Doctor Not Found");
            }
        }


        [HttpGet("Doctor")]
        public async Task<IActionResult> GetAllDoctors(int pagenumber, int pagesize,string? search=null)
        {
            PageResult<Doctor> doctors = unitOfWork.Doctor.GetAll(pagenumber, pagesize, "Specialization,user", search);
            List<GetDoctorDTO> doctorDTO = new List<GetDoctorDTO>();
            foreach (var doctor in doctors.Data)
            {
                //var image = unitOfWork.Image.GetById((int)doctor.user.UserImageId);

                    doctorDTO.Add(
                    new GetDoctorDTO() {
                        ImageId = (int)doctor.user.UserImageId,
                        FullName = doctor.user.FirstName + " " + doctor.user.LastName,
                        Email = doctor.user.Email,
                        Phone = doctor.user.PhoneNumber,
                        SpecializeName = doctor.Specialization.Name,
                        Gender = doctor.user.Gender
                     }); 
            }
            return Ok(doctorDTO);
        }
        //////////////////////////////////////////
        ///

        //////////////////////////////////////////// Patient ////////////////////////

        [HttpGet("Patient")]
        public async Task< IActionResult> GetAllPatient(int pagenumber, int pagesize,string? search=null)
        {
            PageResult<ApplicationUser> patients = unitOfWork.AppUser.GetAll(pagenumber, pagesize, null, search);
            List<GetPatientDTO> patientDTO = new List<GetPatientDTO>();
            foreach (var patient in patients.Data)
            {
                patientDTO.Add(
                new GetPatientDTO()
                {
                    ImageId = (int)patient.UserImageId,
                    FullName = patient.FirstName + " " + patient.LastName,
                    Email = patient.Email,
                    Phone = patient.PhoneNumber,
                    BirthDate = patient.Birthdate,
                    Gender = patient.Gender
                });
            }
            return Ok(patientDTO);
        }


        [HttpGet("Patient/{id}")]
        public async Task<IActionResult> GetPatientById(string id)
        {
            ApplicationUser patient = unitOfWork.AppUser.GetById(id); //user,coupon
            var CheckPatient = userManager.IsInRoleAsync(patient, WebSiteRoles.SitePatient).GetAwaiter().GetResult();
            
            if(!CheckPatient)
            {
                return BadRequest("User isnt a Patient");
            }

            List<Booking> patientRequests = unitOfWork.Booking.GetAll(b => b.userId == id, null, "Coupon,Oppintment,Doctor,Specialization,Time").ToList(); // booking,
            // doctor details
            GetPatientDTO patientDTO = new GetPatientDTO()
            {
                ImageId = (int)patient.UserImageId,
                FullName = patient.FirstName + " " + patient.LastName,
                Email = patient.Email,
                Phone = patient.PhoneNumber,
                Gender = patient.Gender,
                BirthDate = patient.Birthdate
            };

            //feedback
            List<PatientsRequestsDTO> patientsRequestsDTO = new List<PatientsRequestsDTO>();
            foreach (var patientrequest in patientRequests)
            {
                decimal fPrice = 0;
                //var image = unitOfWork.Image.GetById((int)doctor.user.UserImageId);
                if((int)patientrequest.Coupon.DiscountType == 1) // %
                {
                    fPrice = (patientrequest.Coupon.DiscountValue / 100) * patientrequest.Doctor.Price;
                }
                else if((int)patientrequest.Coupon.DiscountType == 2)
                {
                    fPrice = patientrequest.Doctor.Price - (patientrequest.Coupon.DiscountValue) ;

                }
                //
                var DoctorUser = await userManager.FindByIdAsync(patientrequest.Doctor.userId);

                patientsRequestsDTO.Add(
                new PatientsRequestsDTO()
                {
                    DoctorImageId = (int)DoctorUser.UserImageId, //(int)patientrequest.Doctor.user.UserImageId,
                    DoctorName = DoctorUser.FirstName + " " + DoctorUser.FirstName, //patientrequest.Doctor.user.FirstName + " " + patientrequest.Doctor.user.FirstName,
                    SpecalizeName = patientrequest.Specialization.Name,
                    Day = Enum.GetName(typeof(Days), patientrequest.Oppintment.Days) ,
                    Time = patientrequest.Time.Time,
                    Price = patientrequest.Doctor.Price,
                    FinalPrice = fPrice,
                    discoundCode = patientrequest.Coupon.CouponCode,
                    status = Enum.GetName(typeof(Days), patientrequest.RequestStatus) 

                });
            }

            if (patient != null)
            {
                return Ok(new
                {
                    details = patientDTO,
                    requests = patientsRequestsDTO
                });
            }
            else
            {
                return BadRequest("Patient Not Found");
            }
        }



        //////////////////////////////////////////// Coupon ////////////////////////
        ///

        [HttpPost("Coupon")]
        public IActionResult AddCoupon(AddCouponDTO couponDto)
        {
            if(ModelState.IsValid)
            {
                Coupon coupon = new Coupon()
                {
                    CouponCode = couponDto.CouponCode,
                    NumOfBooking = couponDto.NumOfBooking,
                    DiscountType = couponDto.DiscountType,
                    DiscountValue = couponDto.DiscountValue,
                    IsActieve = couponDto.IsActieve
                };
                
                Coupon c = unitOfWork.Coupon.Create(coupon);
                if(c!=null)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("SomeThing Wrong");
                }

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut("Coupon/{id}")]
        public IActionResult EditCoupon(int id , AddCouponDTO couponDto)
        {
            if(ModelState.IsValid)
            {
                Coupon coupon = unitOfWork.Coupon.GetById(id);
                if(coupon!=null)
                {
                    var bookingsCount = unitOfWork.Booking.GetAll(b => b.CouponId == coupon.Id).ToList().Count();
                    if(bookingsCount > 0)
                    {
                        return BadRequest("cant Update this Coupon");
                    }
                    else
                    {
                        coupon.CouponCode = couponDto.CouponCode;
                        coupon.NumOfBooking = couponDto.NumOfBooking;
                        coupon.DiscountType = couponDto.DiscountType;
                        coupon.DiscountValue = couponDto.DiscountValue;
                        coupon.IsActieve = couponDto.IsActieve;

                        bool res = unitOfWork.Coupon.Update(coupon);

                        if (res)
                        {
                            return Ok(true);
                        }
                        else
                        {
                            return BadRequest("cant update coupon");
                        }
                    }
                }
                else
                {
                    return BadRequest("coupon Doesnt Exists");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("Coupon/{id}")]
        public IActionResult DeleteCoupon(int id)
        {
            Coupon coupon = unitOfWork.Coupon.GetById(id);

            if(coupon !=null)
            {
                bool res = unitOfWork.Coupon.Delete(coupon);

                if (res)
                {
                    return Ok(true);
                }
                return BadRequest("cant Delete Coupon");
            }
            else
            {
                return BadRequest("Coupon doesnt exist");
            }

        }

        [HttpPut("CouponDeactivate/{id}")]
        public IActionResult DeactivateCoupon(int id)
        {
            Coupon coupon = unitOfWork.Coupon.GetById(id);
            if (coupon != null)
            {
                coupon.IsActieve = 0;
                unitOfWork.Save();
                return Ok(true);
            }
            else
            {
                return BadRequest("Coupon doesnt exist");
            }

        }
    }
}

