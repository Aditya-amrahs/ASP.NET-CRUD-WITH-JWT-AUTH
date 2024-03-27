using CRUDinNETCORE.Helpers;
using CRUDinNETCORE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDinNETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Brandcontroller : ControllerBase
    {
        private readonly BrandContext _dbContext;
        public Brandcontroller(BrandContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            return await _dbContext.Brands.ToListAsync();
        }


        [HttpGet("{ID}")]
        public async Task<ActionResult<Brand>> GetBrand(int ID)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _dbContext.Brands.FindAsync(ID);
            if(brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new{ID = brand.ID}, brand);
        }

        [HttpPut]
        public async Task<ActionResult> PutBrand(int ID, Brand brand)
        {
            if(ID != brand.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!BrandAvailable(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return Ok();
        }

        private bool BrandAvailable(int ID)
        {
            return (_dbContext.Brands?.Any(x => x.ID== ID)).GetValueOrDefault();
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteBrand(int ID)
        {
            if(_dbContext.Brands == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Brands.FindAsync(ID);
            
            if(brand == null)
            {
                return NotFound();
            }

            _dbContext.Brands.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return Ok();

        }
    
    
        
    
    } 
}