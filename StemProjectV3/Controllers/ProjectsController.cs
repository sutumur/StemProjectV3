using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StemProjectV3.Data;
using StemProjectV3.Models;
using StemProjectV3.Models.SchoolViewModels;

namespace StemProjectV3.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly SchoolContext _context;

        public ProjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Projects
        //public async Task<IActionResult> Index()
        //{
        //    var projects = _context.Projects
        //        .AsNoTracking();

        //    return View(await projects.ToListAsync());
        //}

        public async Task<IActionResult> Index(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
           
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var viewModel = new ProjectIndexData();
            viewModel.Projects = await _context.Projects
                .Include(p => p.ProjectAssignments)
                    .ThenInclude(p => p.Mentor)
                .Include(p => p.Enrollments)
                    .ThenInclude(p => p.Student)
                .AsNoTracking()
                .OrderBy(p => p.ProjectID)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel.Projects = viewModel.Projects.Where(p => p.Name.Contains(searchString) || p.Abstract.Contains(searchString) || p.ProjectID == Convert.ToInt32(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    viewModel.Projects = viewModel.Projects.OrderByDescending(p => p.Name);
                    break;
                case "Date":
                    viewModel.Projects = viewModel.Projects.OrderBy(p => p.ProjectDate);
                    break;
                case "date_desc":
                    viewModel.Projects = viewModel.Projects.OrderByDescending(p => p.ProjectDate);
                    break;
                default:
                    viewModel.Projects =  viewModel.Projects.OrderBy(p => p.ProjectID);
                    break;
            }

            if(id != null)
            {
                ViewData["ProjectID"] = id.Value;
                Project project = viewModel.Projects.Where(p => p.ProjectID == id.Value).Single();
                viewModel.Assignments = project.ProjectAssignments;
                viewModel.Enrollments = project.Enrollments;
            }

            int pageSize = 3;

            return View(viewModel);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.ProjectAssignments).ThenInclude(m => m.Mentor)
                .Include(p => p.Enrollments).ThenInclude(s => s.Student)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ProjectID == id);
            if(project == null)
            {
                return NotFound();
            }

            PopulateAssignedStudentData(project);
            PopulateAssignedMentorData(project);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedMentors, string[] selectedStudents)
        {
            if(id == null)
            {
                return NotFound();
            }

            var projectToUpdate = await _context.Projects
                .Include(p => p.ProjectAssignments)
                    .ThenInclude(p => p.Mentor)
                .Include(p=>p.Enrollments)
                    .ThenInclude(p=>p.Student)
                .SingleOrDefaultAsync(m => m.ProjectID == id);

            if (await TryUpdateModelAsync<Project>(projectToUpdate, "", i => i.Name, i=> i.Abstract, i=>i.ProjectDate))
            {
                UpdateProjectMentors(selectedMentors, projectToUpdate);
                UpdateProjectStudents(selectedStudents, projectToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your administrator.");
                }

                return RedirectToAction(nameof(Index));
            }
            UpdateProjectMentors(selectedMentors, projectToUpdate);
            UpdateProjectStudents(selectedStudents, projectToUpdate);
            PopulateAssignedMentorData(projectToUpdate);
            PopulateAssignedStudentData(projectToUpdate);
            return View(projectToUpdate);
        }

        private void UpdateProjectMentors(string[] selectedMentors, Project projectToUpdate)
        {
            if(selectedMentors == null)
            {
                projectToUpdate.ProjectAssignments = new List<ProjectAssignment>();
                return;
            }

            var selectedMentorHS = new HashSet<string>(selectedMentors);
            var projectMentors = new HashSet<int>(projectToUpdate.ProjectAssignments.Select(p => p.Mentor.ID));
            foreach(var mentor in _context.Mentors)
            {
                if (selectedMentorHS.Contains(mentor.ID.ToString()))
                {
                    if (!projectMentors.Contains(mentor.ID))
                    {
                        projectToUpdate.ProjectAssignments.Add(new ProjectAssignment { ProjectID = projectToUpdate.ProjectID, MentorID = mentor.ID });
                    }
                }
                else
                {
                    if (projectMentors.Contains(mentor.ID))
                    {
                        ProjectAssignment projectAssignment = projectToUpdate.ProjectAssignments.SingleOrDefault(i => i.MentorID == mentor.ID);
                        _context.Remove(projectAssignment);
                    }
                }
            }
        }

        private void UpdateProjectStudents(string[] selectedStudents, Project projectToUpdate)
        {
            if(selectedStudents == null)
            {
                projectToUpdate.Enrollments = new List<Enrollment>();
                return;
            }

            var selectedStudentHS = new HashSet<string>(selectedStudents);
            var projectStudents = new HashSet<int>(projectToUpdate.Enrollments.Select(p => p.Student.ID));
            foreach(var student in _context.Students)
            {
                if (selectedStudentHS.Contains(student.ID.ToString()))
                {
                    if (!projectStudents.Contains(student.ID))
                    {
                        projectToUpdate.Enrollments.Add(new Enrollment { ProjectID = projectToUpdate.ProjectID, StudentID = student.ID });
                    }
                }
                else
                {
                    if (projectStudents.Contains(student.ID))
                    {
                        Enrollment enrollment = projectToUpdate.Enrollments.SingleOrDefault(i => i.StudentID == student.ID);
                        _context.Remove(enrollment);
                    }
                }
            }
        }

        private void PopulateAssignedMentorData(Project project)
        {
            var allMentors = _context.Mentors;
            var mentorProjects = new HashSet<int>(project.ProjectAssignments.Select(m => m.MentorID));
            var viewModel = new List<AssignedMentorData>();
            foreach(var mentor in allMentors)
            {
                viewModel.Add(new AssignedMentorData
                {
                    ID = mentor.ID,
                    FirstName = mentor.FirstName,
                    LastName = mentor.LastName,
                    Assigned = mentorProjects.Contains(mentor.ID)
                });
            }
            ViewData["Mentors"] = viewModel;
        }

        private void PopulateAssignedStudentData(Project project)
        {
            var allStudents = _context.Students;
            var studentEnrollments = new HashSet<int>(project.Enrollments.Select(s => s.StudentID));
            var viewModel = new List<AssignedStudentData>();
            foreach(var student in allStudents)
            {
                viewModel.Add(new AssignedStudentData
                {
                    ID = student.ID,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Assigned = studentEnrollments.Contains(student.ID)
                });
            }
            ViewData["Students"] = viewModel;
        }

        public IActionResult Create()
        {
            var project = new Project();
            project.ProjectAssignments = new List<ProjectAssignment>();
            project.Enrollments = new List<Enrollment>();
            PopulateAssignedMentorData(project);
            PopulateAssignedStudentData(project);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name", "Abstract", "ProjectDate")] Project project, string[] selectedMentors, string[] selectedStudents)
        {
            if(selectedMentors != null)
            {
                project.ProjectAssignments = new List<ProjectAssignment>();
                foreach(var mentor in selectedMentors)
                {
                    var mentorToAdd = new ProjectAssignment { ProjectID = project.ProjectID, MentorID = int.Parse(mentor) };
                    project.ProjectAssignments.Add(mentorToAdd);
                }
            }

            if(selectedStudents != null)
            {
                project.Enrollments = new List<Enrollment>();
                foreach(var student in selectedStudents)
                {
                    var studentToAdd = new Enrollment { ProjectID = project.ProjectID, StudentID = int.Parse(student) };
                    project.Enrollments.Add(studentToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateAssignedMentorData(project);
            PopulateAssignedStudentData(project);
            return View(project);
        }
        //// GET: Projects/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Projects/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProjectID,Name,Abstract,ProjectDate")] Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(project);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(project);
        //}

        //// GET: Projects/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var project = await _context.Projects.SingleOrDefaultAsync(m => m.ProjectID == id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(project);
        //}

        //// POST: Projects/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ProjectID,Name,Abstract,ProjectDate")] Project project)
        //{
        //    if (id != project.ProjectID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(project);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProjectExists(project.ProjectID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(project);
        //}

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Project project = await _context.Projects
                .Include(i => i.ProjectAssignments)
                .Include(i => i.Enrollments)
                .SingleAsync(i => i.ProjectID == id);
            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //var project = await _context.Projects.SingleOrDefaultAsync(m => m.ProjectID == id);
            //_context.Projects.Remove(project);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
    }
}
