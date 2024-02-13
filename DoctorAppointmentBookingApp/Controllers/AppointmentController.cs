using DoctorAppointmentBookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DoctorAppointmentBooking.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly string _jsonFilePath = "appointments.json";

        public IActionResult Index()
        {
            List<Appointment> appointments = GetAppointmentsFromJson();
            return View(appointments);
        }

        public IActionResult Book()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Book(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return View(appointment);
            }

            List<Appointment> appointments = GetAppointmentsFromJson();

            if (appointments.Any(a => a.Time == appointment.Time))
            {
                ModelState.AddModelError("VisitTime", "Appointment time is already booked.");
                return View(appointment);
            }

            if (appointment.Time.TimeOfDay < TimeSpan.FromHours(10) || appointment.Time.TimeOfDay > TimeSpan.FromHours(19))
            {
                ModelState.AddModelError("VisitTime", "Appointment time must be between 10:00 and 19:00.");
                return View(appointment);
            }

            appointments.Add(appointment);
            SaveAppointmentsToJson(appointments);

            return RedirectToAction(nameof(Index));
        }

        private List<Appointment> GetAppointmentsFromJson()
        {
            if (!System.IO.File.Exists(_jsonFilePath))
            {
                return new List<Appointment>();
            }

            string jsonData = System.IO.File.ReadAllText(_jsonFilePath);
            return JsonConvert.DeserializeObject<List<Appointment>>(jsonData);
        }

        private void SaveAppointmentsToJson(List<Appointment> appointments)
        {
            string jsonData = JsonConvert.SerializeObject(appointments);
            System.IO.File.WriteAllText(_jsonFilePath, jsonData);
        }
    }
}
