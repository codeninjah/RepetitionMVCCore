using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repetition.Data;
using Repetition.Models;

namespace Repetition.Controllers
{
	public class SongsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<SongsController> _logger;

		public SongsController(ApplicationDbContext context, ILogger<SongsController> logger)
		{
			_logger = logger;
			_context = context;
			_context.Songs.Include(x => x.Artist)
				.Load();
		}

		public async Task<IActionResult> Artist(int? id)
		{
			if (!id.HasValue)
			{
				//_logger.LogWarning("Details: Could not find post with ID = " + id.ToString());
				return RedirectToAction("Index");
			}
			else
			{
				var _model = await _context.Songs
					.Where(x => x.ArtistID == id)
					?.ToListAsync();

				var test = _context.Artist.Single(a => a.ArtistID == id);

				_logger.LogWarning("Details: Artist " + test.Name);
				return View(_model);
			}
		}

		// GET: Songs
		public async Task<IActionResult> Index()
		{
			return View(await _context.Songs.ToListAsync());
		}


		// GET: Songs/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var songs = await _context.Songs
				.SingleOrDefaultAsync(m => m.SongID == id);
			if (songs == null)
			{
				return NotFound();
			}

			return View(songs);
		}

		// GET: Songs/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Songs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("SongsID,Name")] Song songs)
		{
			if (ModelState.IsValid)
			{
				_context.Add(songs);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(songs);
		}

		// GET: Songs/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var songs = await _context.Songs.SingleOrDefaultAsync(m => m.SongID == id);
			if (songs == null)
			{
				return NotFound();
			}
			return View(songs);
		}

		// POST: Songs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("SongsID,Name")] Song songs)
		{
			if (id != songs.SongID)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(songs);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!SongsExists(songs.SongID))
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
			return View(songs);
		}

		// GET: Songs/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var songs = await _context.Songs
				.SingleOrDefaultAsync(m => m.SongID == id);
			if (songs == null)
			{
				return NotFound();
			}

			return View(songs);
		}

		// POST: Songs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var songs = await _context.Songs.SingleOrDefaultAsync(m => m.SongID == id);
			_context.Songs.Remove(songs);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool SongsExists(int id)
		{
			return _context.Songs.Any(e => e.SongID == id);
		}
	}
}
