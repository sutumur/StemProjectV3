using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StemProjectV3.Data;
using StemProjectV3.Models.SchoolViewModels;

namespace StemProjectV3.Models
{
    public class MentorsController : Controller
    {
        private readonly SchoolContext _context;

        public MentorsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Mentors
        public async Task<IActionResult> Index(int? id, int? projectID)
        {
            var viewModel = new MentorIndexData();
            viewModel.Mentors = await _context.Mentors
                .Include(i => i.ProjectAssignments)
                    .ThenInclude(i => i.Project)
                        .ThenInclude(i => i.Enrollments)
                            .ThenInclude(i => i.Student)
                .Include(i => i.ProjectAssignments)
                    .ThenInclude(i => i.Project)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if(id != null)
            {
                ViewData["MentorID"] = id.Value;
                Mentor mentor = viewModel.Mentors.Where(i => i.ID == id.Value).Single();
                viewModel.Projects = mentor.ProjectAssignments.Select(s => s.Project);
            }

            if(projectID != null)
            {
                ViewData["ProjectID"] = projectID.Value;
                viewModel.Enrollments = viewModel.Projects.Where(x => x.ProjectID == projectID).Single().Enrollments;
            }

            return View(viewModel);
        }

        // GET: Mentors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await _context.Mentors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (mentor == null)
            {
                return NotFound();
            }

            return View(mentor);
        }

        // GET: Mentors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mentors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,Email,SupportArea,Discipline,Comment")] Mentor mentor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mentor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mentor);
        }

        // GET: Mentors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await _context.Mentors.SingleOrDefaultAsync(m => m.ID == id);
            if (mentor == null)
            {
                return NotFound();
            }
            return View(mentor);
        }

        // POST: Mentors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,Email,SupportArea,Discipline,Comment")] Mentor mentor)
        {
            if (id != mentor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mentor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MentorExists(mentor.ID))
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
            return View(mentor);
        }

        // GET: Mentors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentor = await _context.Mentors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (mentor == null)
            {
                return NotFound();
            }

            return View(mentor);
        }

        // POST: Mentors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mentor = await _context.Mentors.SingleOrDefaultAsync(m => m.ID == id);
            _context.Mentors.Remove(mentor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MentorExists(int id)
        {
            return _context.Mentors.Any(e => e.ID == id);
        }
    }
}
