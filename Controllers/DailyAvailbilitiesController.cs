using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doctor_Appointment.Data;
using Doctor_Appointment.Repo;
using Doctor_Appointment.Models;
using Microsoft.AspNetCore.Authorization;

namespace Doctor_Appointment.Controllers
{
    [Authorize (Roles ="Doctor")]
    public class DailyAvailbilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IDailyAvailbilityRepo Daily { get; }

        public DailyAvailbilitiesController(ApplicationDbContext context , IDailyAvailbilityRepo daily)
        {
            _context = context;
            Daily = daily;
        }

        // GET: DailyAvailbilities
        public async Task<IActionResult> Index(int docid)
        {
            try
            {

                return View(Daily.GetAll(docid));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return View();
        }

        // GET: DailyAvailbilities/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.date = _context.dailyAvailbilities.Select(a => a.Dayid);

            if (_context.dailyAvailbilities == null)
            {
                return NotFound();
            }

            var dailyAvailbility = await _context.dailyAvailbilities
                .FirstOrDefaultAsync(m => m.DoctorID == id);

            if (dailyAvailbility == null)
            {
                return NotFound();
            }
            Daily.GetById(id);

            return View("Details", dailyAvailbility);
        }

        // GET: DailyAvailbilities/Create
        public IActionResult Create()
        {
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "FullName");
            return View();
        }

        // POST: DailyAvailbilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoctorID,Dayid,date,starttime,endtime,Isavailable")] DailyAvailbility dailyAvailbility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyAvailbility);
                await _context.SaveChangesAsync();
                return View("Details", dailyAvailbility);
            }
            return View(dailyAvailbility);
        }

        // GET: DailyAvailbilities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            //ViewBag.DoctorID = _context.Doctors.Where(d=>d.DoctorID==id).Select(d=>d.FullName).ToString();

            ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "FullName");

            if ( _context.dailyAvailbilities == null)
            {
                return NotFound();
            }

            var dailyAvailbility = await _context.dailyAvailbilities.FindAsync(id);
            if (dailyAvailbility == null)
            {
                return NotFound();
            }
            return View(dailyAvailbility);
        }

        // POST: DailyAvailbilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,Dayid,date,starttime,endtime,Isavailable")] DailyAvailbility dailyAvailbility)
        {
            if (id != dailyAvailbility.Dayid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyAvailbility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyAvailbilityExists(dailyAvailbility.Dayid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dailyAvailbility);
        }

        // GET: DailyAvailbilities/Delete/5
        public IActionResult Delete(int id)
        {
            if (_context.dailyAvailbilities == null)
            {
                return NotFound();
            }

            var dailyAvailbility = _context.dailyAvailbilities.Include(a => a.DoctorID)
                .FirstOrDefaultAsync(m => m.Dayid == id);
            if (dailyAvailbility == null)
            {
                return NotFound();
            }
            return View(dailyAvailbility);
        }

        // POST: DailyAvailbilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.dailyAvailbilities == null)
            {
                return Problem("Entity set 'MedcareDbContext.dailyAvailbilities'  is null.");
            }
            var dailyAvailbility = await _context.dailyAvailbilities.FindAsync(id);
            if (dailyAvailbility != null)
            {
                _context.dailyAvailbilities.Remove(dailyAvailbility);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyAvailbilityExists(int id)
        {
          return (_context.dailyAvailbilities?.Any(e => e.Dayid == id)).GetValueOrDefault();
        }
    }
}
