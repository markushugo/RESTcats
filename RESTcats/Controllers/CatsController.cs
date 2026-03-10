using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RESTcats.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTcats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatsController : ControllerBase
    {
        private CatsRepositoryList _repo;

        public CatsController(CatsRepositoryList repo)
        {
            _repo = repo;
        }

        // GET: api/<CatsController>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpGet]
        public ActionResult< IEnumerable<Cat>> Get([FromQuery] int? minimumweight, [FromQuery] int? maximumweight)
        {
            if (minimumweight.HasValue && maximumweight.HasValue && minimumweight > maximumweight)
            {
                return BadRequest();
            }
            IEnumerable<Cat> result = _repo.GetAllCats(minimumweight, maximumweight);
            if (result == null || result.Count() == null) {
                return NoContent();
            }
            return Ok(result);

        }

        // GET api/<CatsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Cat> Get(int id)
        {
            Cat? cat = _repo.GetCatById(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        // POST api/<CatsController>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public ActionResult<Cat> Post([FromBody] Cat newCat)
        {
            try
            {
                _repo.AddCat(newCat);
                return Created($"api/items/{newCat.Id}", newCat);
            }
            catch (ArgumentException ex)
            {
                if (newCat.Name == null) ; 
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CatsController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public ActionResult<Cat> Put(int id, [FromBody] Cat value)
        {
            Cat? cat = _repo.UpdateCat(id, value);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        // DELETE api/<CatsController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Cat> Delete(int id)
        {
            Cat? cat = _repo.RemoveCat(id);
            if (cat == null)
            {
                return NotFound();
            }
            return Ok(cat);
        }

        [HttpOptions]
        public void Options()
        {

        }
    }
}
