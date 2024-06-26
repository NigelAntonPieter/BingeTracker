﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BingeTrackerApi.Data;
using BingeTrackerApi.Model;

namespace BingeTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StreamingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Streamings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Streaming>>> GetStreamings()
        {
          if (_context.Streamings == null)
          {
              return NotFound();
          }
            return await _context.Streamings.ToListAsync();
        }

        // GET: api/Streamings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Streaming>> GetStreaming(int id)
        {
          if (_context.Streamings == null)
          {
              return NotFound();
          }
            var streaming = await _context.Streamings.FindAsync(id);

            if (streaming == null)
            {
                return NotFound();
            }

            return streaming;
        }

        // PUT: api/Streamings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStreaming(int id, Streaming streaming)
        {
            if (id != streaming.Id)
            {
                return BadRequest();
            }

            _context.Entry(streaming).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreamingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Streamings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Streaming>> PostStreaming(Streaming streaming)
        {
          if (_context.Streamings == null)
          {
              return Problem("Entity set 'AppDbContext.Streamings'  is null.");
          }
            _context.Streamings.Add(streaming);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStreaming", new { id = streaming.Id }, streaming);
        }

        // DELETE: api/Streamings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreaming(int id)
        {
            if (_context.Streamings == null)
            {
                return NotFound();
            }
            var streaming = await _context.Streamings.FindAsync(id);
            if (streaming == null)
            {
                return NotFound();
            }

            _context.Streamings.Remove(streaming);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StreamingExists(int id)
        {
            return (_context.Streamings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
