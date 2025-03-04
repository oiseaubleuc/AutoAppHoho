using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoAppHoho.Data;
using AutoAppHoho.Models;

namespace AutoAppHoho.Controllers.Api
{
    [Route("api/cars")]
    [ApiController]
    public class CarsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound(new { message = "Auto niet gevonden" });
            }

            return car;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> PostCar([FromBody] Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest(new { message = "ID komt niet overeen" });
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.Id == id))
                {
                    return NotFound(new { message = "Auto niet gevonden" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound(new { message = "Auto niet gevonden" });
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
