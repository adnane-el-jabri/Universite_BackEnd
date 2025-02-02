using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Dtos;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;
using UniversiteDomain.UseCases.UeUseCases.Get;
using UniversiteDomain.UseCases.UeUseCases.Update;
using UniversiteDomain.UseCases.UeUseCases.Delete;
using UniversiteDomain.UseCases.SecurityUseCases.Get;

namespace UniversiteRestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UeApiController : ControllerBase
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly CreateUeUseCase _createUeUseCase;

        public UeApiController(IRepositoryFactory repositoryFactory, CreateUeUseCase createUeUseCase)
        {
            _repositoryFactory = repositoryFactory;
            _createUeUseCase = createUeUseCase;
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
        public async Task<ActionResult<List<UeDto>>> GetAllUes()
        {
            var ues = await _repositoryFactory.UeRepository().GetAllAsync();
            return Ok(UeDto.ToDtos(ues));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UeDto>> GetUneUe(long id)
        {
            var ue = await _repositoryFactory.UeRepository().GetByIdAsync(id);
            if (ue == null)
            {
                return NotFound();
            }
            return Ok(new UeDto().ToDto(ue));
        }

        [HttpPost]
        public async Task<ActionResult<UeDto>> PostAsync([FromBody] UeDto ueDto)
        {
            if (ueDto == null)
            {
                return BadRequest();
            }

            string role = "";
            string email = "";
            IUniversiteUser user = null;

            CheckSecu(out role, out email, out user);

            if (!_createUeUseCase.IsAuthorized(role))
                return Unauthorized();

            Ue ue = ueDto.ToEntity();

            try
            {
                ue = await _createUeUseCase.ExecuteAsync(ue);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }

            UeDto dto = new UeDto().ToDto(ue);
            return CreatedAtAction(nameof(GetUneUe), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(long id, [FromBody] UeDto ueDto)
        {
            if (ueDto == null || id != ueDto.Id)
            {
                return BadRequest();
            }

            UpdateUeUseCase updateUeUc = new UpdateUeUseCase(_repositoryFactory);

            string role = "";
            string email = "";
            IUniversiteUser user = null;
            CheckSecu(out role, out email, out user);

            if (!updateUeUc.IsAuthorized(role)) return Unauthorized();

            try
            {
                await updateUeUc.ExecuteAsync(ueDto.ToEntity());
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
            DeleteUeUseCase ueUc = new DeleteUeUseCase(_repositoryFactory);

            string role = "";
            string email = "";
            IUniversiteUser user = null;
            CheckSecu(out role, out email, out user);

            if (!ueUc.IsAuthorized(role)) return Unauthorized();

            try
            {
                await ueUc.ExecuteAsync(id);
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