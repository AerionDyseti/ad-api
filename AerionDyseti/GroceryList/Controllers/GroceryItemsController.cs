using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDyseti.GroceryList.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/GroceryItems")]
    public class GroceryItemsController : Controller
    {
        private readonly AerionDysetiContext _context;

        public GroceryItemsController(AerionDysetiContext context)
        {
            _context = context;
        }

        // GET: api/GroceryItems
        [HttpGet]
        public IEnumerable<GroceryItem> GetGroceryItems()
        {
            return _context.GroceryItems;
        }

        // GET: api/GroceryItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroceryItem([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groceryItem = await _context.GroceryItems.SingleOrDefaultAsync(m => m.Id == id);

            if (groceryItem == null)
            {
                return NotFound();
            }

            return Ok(groceryItem);
        }

        // PUT: api/GroceryItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroceryItem([FromRoute] long id, [FromBody] GroceryItem groceryItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groceryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(groceryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroceryItemExists(id))
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

        // POST: api/GroceryItems
        [HttpPost]
        public async Task<IActionResult> PostGroceryItem([FromBody] GroceryItem groceryItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GroceryItems.Add(groceryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroceryItem", new { id = groceryItem.Id }, groceryItem);
        }

        // DELETE: api/GroceryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroceryItem([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groceryItem = await _context.GroceryItems.SingleOrDefaultAsync(m => m.Id == id);
            if (groceryItem == null)
            {
                return NotFound();
            }

            _context.GroceryItems.Remove(groceryItem);
            await _context.SaveChangesAsync();

            return Ok(groceryItem);
        }

        private bool GroceryItemExists(long id)
        {
            return _context.GroceryItems.Any(e => e.Id == id);
        }
    }
}