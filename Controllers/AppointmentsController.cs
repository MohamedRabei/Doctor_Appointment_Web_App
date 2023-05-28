using Doctor_Appointment.Data;
using Doctor_Appointment.Models;
using Doctor_Appointment.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Appointment.Controllers
{
    [Authorize(Roles = "Patient")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _user;

        public IAppointmentRepo Repo { get; }

        public AppointmentsController(ApplicationDbContext context, IAppointmentRepo repo, SignInManager<IdentityUser> manager, UserManager<IdentityUser> user)
        {
            _context = context;
            Repo = repo;
            _signInManager = manager;
            _user = user;
        }

        // GET: Appointments

        [Authorize]
        public IActionResult Index()
        {
            var patId = _context.Patients.First(p => p.IdentityId == User.Claims.First().Value).PatientID;

            ViewBag.date = _context.Appointments.Select(a => a.dailyAvailbility.Dayid);

            return View(Repo.GetAll(patId));

        }

        // GET: Appointments/Details/5
        public IActionResult Details(int id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = _context.Appointments
                .Include(a => a.doctor)
                .Include(a => a.patient)
                .Where(m => m.appointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(Repo.GetById(id));
        }

        // GET: Appointments/Create
        public IActionResult Create(int Docid)
        {
            //ViewData["DoctorID"] = new SelectList(_context.Doctors.Include(d=>d.availableDays), "DoctorID", "FullName");
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "FullName");

            //var DoctorDates = _context.Doctors.Include(d => d.availableDays)
            //    .Select(d=> new {docId=d.DoctorID ,DocName=d.FullName , dates=d.availableDays
            //    .Select(a=>a.date).Distinct()}).ToList();

            ViewBag.DoctorDates = _context.Doctors.Include(d => d.availableDays)
            .FirstOrDefault(d => d.DoctorID == Docid);

            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "FullName");
            var patient = _context.Patients.First(p => p.IdentityId == User.Claims.First().Value);
            ViewData["PatientID"] = patient.PatientID;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Repo.Insert(appointment);

                    return RedirectToAction("Index", "PayPal");

                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            else
                return NotFound();
        }

        // GET: Appointments/Edit/5
        public IActionResult Edit(int id)
        {
            ViewBag.date = _context.Appointments.Select(a => a.dailyAvailbility.Dayid);

            if (_context.Appointments == null)
            {
                return NotFound();
            }

            var appointment = _context.Appointments.FirstOrDefault(a => a.appointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DoctorID,PatientID,AppointmentDay,AppointmentTime,AppointmentType,MedicalHistory")] Appointment appointment)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    Repo.Update(appointment.DoctorID, appointment.PatientID, appointment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int Docid, int Patid)
        {
            ViewBag.date = _context.Appointments.Select(a => a.dailyAvailbility.Dayid);

            var appointment = await _context.Appointments
                .Include(a => a.doctor)
                .Include(a => a.patient)
                .FirstOrDefaultAsync(m => m.DoctorID == Docid && m.PatientID == Patid);

            if (appointment != null)
            {
                Repo.Delete(appointment.DoctorID, appointment.PatientID);
            }

            return View();
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int DocId, int PatId)
        {
            //if (_context.Appointments == null)
            //{
            //    return Problem("Entity set 'MedcareDbContext.Appointments'  is null.");
            //}
            var appointment = _context.Appointments.FirstOrDefault(m => m.DoctorID == DocId && m.PatientID == PatId);
            if (appointment != null)
            {
                Repo.Delete(DocId, PatId);

            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return (_context.Appointments?.Any(e => e.DoctorID == id)).GetValueOrDefault();
        }
    }
}
