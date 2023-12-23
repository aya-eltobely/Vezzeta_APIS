using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
    [Authorize(Roles = WebSiteRoles.SitePatient)]

    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PatientController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        [HttpGet("Doctor")]
        public async Task<IActionResult> GetAllDoctorsWithAppointment(int pagenumber, int pagesize, string? search = null)
        {
            PageResult<Doctor> doctors = unitOfWork.Doctor.GetAll(pagenumber, pagesize, "Specialization,user", search);  

            List<GetDoctorWithAppointmentDTO> doctorDTO = new List<GetDoctorWithAppointmentDTO>();

            //List<GetAppointmentDTO> AllAppointmentDTOs = new List<GetAppointmentDTO>();

            foreach (var doctor in doctors.Data)
            {

                List<Oppintment> appointment = unitOfWork.Oppintment.GetAll(o => o.DoctorId == doctor.Id).ToList();

                List<GetAppointmentDTO> DoctorAppointmentDTOs = new List<GetAppointmentDTO>();

                foreach (var item in appointment)
                {
                    GetAppointmentDTO appointDTO = new GetAppointmentDTO();
                    List<GetTimeDTO> timesDTO = new List<GetTimeDTO>();
                    appointDTO.Day = item.Days;
                    ///
                    List<Times> times = unitOfWork.Times.GetAll(t => t.OppintmentId == item.Id).ToList(); //new List<Times>();
                    
                    foreach (var time in times)
                    {
                        GetTimeDTO timeDTO = new GetTimeDTO();
                        timeDTO.Id = time.Id;
                        timeDTO.Time = time.Time;
                        timesDTO.Add(timeDTO);

                    }

                    appointDTO.Times = timesDTO;

                    DoctorAppointmentDTOs.Add(appointDTO);
                }

                //AllAppointmentDTOs.Add(DoctorAppointmentDTOs);

                //

                doctorDTO.Add(
                new GetDoctorWithAppointmentDTO()
                {
                    ImageId = (int)doctor.user.UserImageId,
                    FullName = doctor.user.FirstName + " " + doctor.user.LastName,
                    Email = doctor.user.Email,
                    Phone = doctor.user.PhoneNumber,
                    SpecializeName = doctor.Specialization.Name,
                    Gender = doctor.user.Gender,
                    Price = doctor.Price,
                    appointment = DoctorAppointmentDTOs
                });
            }


            return Ok( doctorDTO );
        }


        [HttpGet("Booking")]
        public IActionResult GetAllPatientBooking(string PatientId)
        {
            //var PatientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Booking> patientBookings = unitOfWork.Booking.GetAll(b => b.userId == PatientId, null, "Doctor,Specialization,Oppintment,Coupon,Time").ToList();
            
            if(patientBookings.Count>0)
            {
                List<GetPatientBookingDTO> patientBookingDTOs = new List<GetPatientBookingDTO>();

                foreach (var item in patientBookings)
                {
                    decimal finalprice = 0;

                    if (item.Coupon.DiscountType == DiscountType.Value)
                    {
                        finalprice = item.Doctor.Price - item.Coupon.DiscountValue;
                    }
                    else if (item.Coupon.DiscountType == DiscountType.Percentage)
                    {
                        finalprice = (item.Coupon.DiscountValue / 100) * item.Doctor.Price;
                    }

                    Doctor doctor = unitOfWork.Doctor.GetById((int)item.DoctorId,"user");

                    patientBookingDTOs.Add(new GetPatientBookingDTO()
                    {
                        DoctorImageId = (int)doctor.user.UserImageId,
                        DoctorName = doctor.user.FirstName + " " + doctor.user.FirstName,
                        SpecializationName = doctor.Specialization.Name,
                        Day = item.Oppintment.Days,
                        Time = item.Time.Time,
                        Price = item.Doctor.Price,
                        DiscoundCode = item.Coupon.CouponCode,
                        FinalPrice = finalprice,
                        Status = item.RequestStatus
                    });

                }

                return Ok(patientBookingDTOs);

            }
            else
            {
                return Ok("You Dont Have any Booking");

            }


        }

        //patient Book 
        [HttpPost("AddBooking")]
        public IActionResult AddPatientBooking(AddPatientBookingDTO patientBookingDTO)
        {

            if(ModelState.IsValid)
            {
                Booking newBooking = new Booking();
                newBooking.TimeId = patientBookingDTO.TimeId;
                newBooking.DoctorId = patientBookingDTO.DoctorId;
                newBooking.userId = patientBookingDTO.PatientId;
                newBooking.RequestStatus = RequestStatus.PendingRequests;

                Coupon coupon = unitOfWork.Coupon.GetAll(c => c.CouponCode == patientBookingDTO.discoundCodeCoupon).FirstOrDefault();
                newBooking.CouponId = coupon.Id;

                Oppintment oppintment = unitOfWork.Oppintment.GetAll(o => o.Days == patientBookingDTO.Day && o.DoctorId == patientBookingDTO.DoctorId).FirstOrDefault();
                newBooking.OppintmentId = oppintment.Id;

                Doctor doctor = unitOfWork.Doctor.GetById(patientBookingDTO.DoctorId);
                Specialization specialization = unitOfWork.Specialization.GetById((int)doctor.SpecializationId);
                newBooking.SpecializationId = specialization.Id;


                Booking book = unitOfWork.Booking.Create(newBooking);

                if (book != null)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("SomeThing wrong");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
