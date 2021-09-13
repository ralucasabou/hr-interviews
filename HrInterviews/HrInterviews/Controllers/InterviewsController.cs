using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HrInterviews.Data;
using HrInterviews.Data.Entities;
using HrInterviews.Models;

namespace HrInterviews.Controllers
{
    public class InterviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Interviews
        public async Task<IActionResult> Index()
        {
            List<Interview> interviews = await _context.Interviews.Include(i => i.Profile).ToListAsync();
            List<InterviewViewModel> models = new();
            models.AddRange(
                interviews.Select(e => new InterviewViewModel()
                {
                    InterviewId = e.InterviewId,
                    InterviewDate = e.InterviewDate,
                    Feedback = e.Feedback,
                    Profile = new()
                    {
                        ProfileId = e.Profile.ProfileId,
                        FirstName = e.Profile.FirstName, 
                        LastName = e.Profile.LastName
                    }
                }));

            return View(models);
        }

        // GET: Interviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews
                .FirstOrDefaultAsync(m => m.InterviewId == id);
            if (interview == null)
            {
                return NotFound();
            }

            return View(interview);
        }

        // GET: Interviews/Create
        public async Task<IActionResult> Create()
        {
            InterviewViewModel model = new();
            await EnsureAvailableProfiles(model);
            return View(model);
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SelectedProfileId,InterviewDate,Feedback")] InterviewViewModel interview)
        {
            if (!ModelState.IsValid)
            {
                await EnsureAvailableProfiles(interview);
                return View(interview);
            }

            Profile profileEntity = await _context
                    .Profiles
                    .FirstOrDefaultAsync(p => p.ProfileId == interview.SelectedProfileId);

            if (profileEntity is null)
            {
                ModelState.AddModelError(
                    nameof(InterviewViewModel.SelectedProfileId),
                    "Profile not found");

                await EnsureAvailableProfiles(interview);
                return View(interview);
            }

            Interview interviewEntity = new()
            {
                Profile = profileEntity,
                InterviewDate = interview.InterviewDate,
                Feedback = interview.Feedback
                
            };

            _context.Add(interviewEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Interviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null)
            {
                return NotFound();
            }
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InterviewId,InterviewDate,Feedback")] Interview interview)
        {
            if (id != interview.InterviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(interview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterviewExists(interview.InterviewId))
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
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews
                .FirstOrDefaultAsync(m => m.InterviewId == id);
            if (interview == null)
            {
                return NotFound();
            }

            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var interview = await _context.Interviews.FindAsync(id);
            _context.Interviews.Remove(interview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InterviewExists(int id)
        {
            return _context.Interviews.Any(e => e.InterviewId == id);
        }


        private async Task EnsureAvailableProfiles(
            InterviewViewModel viewModel,
            int? selectedProfileId = null)
        {
            if (viewModel is not null)
            {
                if (selectedProfileId.HasValue)
                {
                    viewModel.SelectedProfileId = selectedProfileId.GetValueOrDefault();
                }

                List<Profile> profileEntities = await _context.Profiles.ToListAsync();
                viewModel.AvailableProfiles.AddRange(profileEntities.Select(
                    e => new SelectListItem
                    {
                        Text = $"{e.FirstName} {e.LastName}",
                        Value = e.ProfileId.ToString(),
                        Selected = selectedProfileId.HasValue &&
                                    selectedProfileId.GetValueOrDefault() == e.ProfileId
                    }));
            }
        }
    }
}
