using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Dtos;
using UniversiteDomain.Entities;

using UniversiteDomain.UseCases.ParcoursUseCases.Get;
using UniversiteDomain.UseCases.ParcoursUseCases.Update;
using UniversiteDomain.UseCases.ParcoursUseCases.Delete;
using UniversiteDomain.UseCases.SecurityUseCases.Get;
using UniversiteEFDataProvider.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversiteRestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParcoursApiController : ControllerBase
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly CreateParcoursUseCase _createParcoursUseCase;

        public ParcoursApiController(IRepositoryFactory repositoryFactory, CreateParcoursUseCase createParcoursUseCase)
        {
            _repositoryFactory = repositoryFactory;
            _createParcoursUseCase = createParcoursUseCase;
        }

        private void CheckSecu(out string role, out string email, out IUniversiteUser user)
        {
            role = "";
            ClaimsPrincipal claims = HttpContext.User;

            if (claims.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException();
            if (claims.FindFirst(ClaimTypes.Email) == null) throw new UnauthorizedAccessException();
            
            email = claims.FindFirst(ClaimTypes.Email).Value;
            if (email == null) throw new UnauthorizedAccessException();
            
            user = new FindUniversiteUserByEmailUseCase(_repositoryFactory).ExecuteAsync(email).Result;
            if (user == null) throw new UnauthorizedAccessException();
            
            if (claims.FindFirst(ClaimTypes.Role) == null) throw new UnauthorizedAccessException();
            
            var ident = claims.Identities.FirstOrDefault();
            if (ident == null) throw new UnauthorizedAccessException();
            
            role = ident.FindFirst(ClaimTypes.Role).Value;
            if (role == null) throw new UnauthorizedAccessException();
            
            bool isInRole = new IsInRoleUseCase(_repositoryFactory).ExecuteAsync(email, role).Result; 
            if (!isInRole) throw new UnauthorizedAccessException();
        }

        [HttpGet]
        public async Task<ActionResult<List<ParcoursDto>>> GetAllParcours()
        {
            var parcours = await _repositoryFactory.ParcoursRepository().GetAllAsync();
            return Ok(ParcoursDto.ToDtos(parcours));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParcoursDto>> GetUnParcours(long id)
        {
            var parcours = await _repositoryFactory.ParcoursRepository().GetByIdAsync(id);
            if (parcours == null)
            {
                return NotFound();
            }
            return Ok(new ParcoursDto().ToDto(parcours));
        }

        [HttpPost]
        public async Task<ActionResult<ParcoursDto>> PostAsync([FromBody] ParcoursDto parcoursDto)
        {
            if (parcoursDto == null)
            {
                return BadRequest();
            }

            string role = "";
            string email = "";
            IUniversiteUser user = null;

            CheckSecu(out role, out email, out user);

            if (!_createParcoursUseCase.IsAuthorized(role))
                return Unauthorized();

            Parcours parcours = parcoursDto.ToEntity();

            try
            {
                parcours = await _createParcoursUseCase.ExecuteAsync(parcours);
            }   
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }

            ParcoursDto dto = new ParcoursDto().ToDto(parcours);
            return CreatedAtAction(nameof(GetUnParcours), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(long id, [FromBody] ParcoursDto parcoursDto)
        {
            if (parcoursDto == null || id != parcoursDto.Id)
            {
                return BadRequest();
            }

            UpdateParcoursUseCase updateParcoursUc = new UpdateParcoursUseCase(_repositoryFactory);

            string role = "";
            string email = "";
            IUniversiteUser user = null;
            CheckSecu(out role, out email, out user);

            if (!updateParcoursUc.IsAuthorized(role)) return Unauthorized();

            try
            {
                await updateParcoursUc.ExecuteAsync(parcoursDto.ToEntity());
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            DeleteParcoursUseCase parcoursUc = new DeleteParcoursUseCase(_repositoryFactory);

            string role = "";
            string email = "";
            IUniversiteUser user = null;
            CheckSecu(out role, out email, out user);

            if (!parcoursUc.IsAuthorized(role)) return Unauthorized();

            try
            {
                await parcoursUc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }

            return NoContent();
        }
    }
}
