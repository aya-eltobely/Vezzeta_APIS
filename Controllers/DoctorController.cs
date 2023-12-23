using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VezetaApi.DTO;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;
using VezetaApi.Services;

namespace VezetaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = WebSiteRoles.SiteDoctor)]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        [HttpGet("Booking")]
        public IActionResult GetAllBooking(int pagenumber, int pagesize,string? search )//, int DocId)
        {

            var doctorId = GetCurrentUser();

            var bookings = unitOfWork.Booking.GetAll(pagenumber, pagesize, "Time,user", doctorId,search );//(b => b.DoctorId == DocId, null, "user").ToList();
            List<GetPatientDTO> patients = new List<GetPatientDTO>();
            
            
            if (bookings.Data != null)
            {

                foreach (var item in bookings.Data)
                {
                    patients.Add(
                        new GetPatientDTO()
                        {
                            ImageId = (int)item.user.UserImageId,
                            FullName = item.user.FirstName + " " + item.user.LastName,
                            Email = item.user.Email,
                            Phone = item.user.PhoneNumber,
                            Gender = item.user.Gender
                        } );
                }
                return Ok(patients);
            }
            else
            {
                return Ok("No Booking In this Date");
            }
        }


        [HttpPut("ConfirmBooking")]
        public IActionResult ConfirmBooking(int BookingId)
        {
            Booking req = unitOfWork.Booking.GetById(BookingId);
            if (req != null)
            {
                req.RequestStatus = RequestStatus.completedRequests;
                unitOfWork.Save();
                return Ok(true);
            }
            else
                return BadRequest("Somting Wrong");
            
        }


        [HttpPost("Appointment")]
        public IActionResult AddAppointment(AddAppointmentDTO appointmentDTO)
        {
            if(ModelState.IsValid)
            {
                var doctorId = GetCurrentUser();


                Doctor doc = unitOfWork.Doctor.GetById(doctorId);
                doc.Price = appointmentDTO.Price;

                Oppintment oppintment = new Oppintment();
                oppintment.Days = appointmentDTO.Day;
                oppintment.DoctorId = doctorId;
                unitOfWork.Oppintment.Create(oppintment);

                List<Times> times = new List<Times>();

                foreach (var item in appointmentDTO.Times)
                {
                    times.Add(
                    new Times()
                    {
                        Time = item,
                        OppintmentId = oppintment.Id
                    });

                }
                unitOfWork.Times.CreateRange(times);
                return Ok(true);
            }
            else
            {
                return BadRequest(ModelState);    
            }

        }

        [HttpPut("Appointment")] 
        public IActionResult EditAppointment(int oppointmentId, string time , int timeId )//,string oldtime, string newtime)
        {
            if(ModelState.IsValid)
            {
                var doctorId = GetCurrentUser();

                Times times = unitOfWork.Times.GetById(timeId);


                var booking = unitOfWork.Booking.GetAll(b => b.DoctorId == doctorId&&b.TimeId==timeId&&b.OppintmentId== oppointmentId).FirstOrDefault();

                if (booking != null)
                {
                    return BadRequest("You cant update on used time");
                }
                else
                {
                    times.Time = time;
                    unitOfWork.Times.Update(times);
                    unitOfWork.Save();

                    return Ok(true);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("Appointment")]
        public IActionResult DeleteAppointment(int oppointmentId , int timeId)
        {
            if (ModelState.IsValid)
            {
                var doctorId = GetCurrentUser();

                Times times = unitOfWork.Times.GetById(timeId);

                var booking = unitOfWork.Booking.GetAll(b => b.DoctorId == doctorId && b.TimeId == timeId && b.OppintmentId == oppointmentId).FirstOrDefault();

                if (booking != null)
                {
                    return Ok("You cant delete on used time");
                }
                else
                {
                    bool res = unitOfWork.Times.Delete(times);
                    if(res)
                        return Ok(true);
                    else
                        return Ok(false);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        private int GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var doctor = unitOfWork.Doctor.GetAll(d => d.userId == userId).ToList().FirstOrDefault();
            var doctorId = doctor.Id;
            return doctorId;
        }
    }
}
