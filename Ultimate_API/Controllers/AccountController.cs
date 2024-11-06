using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{accountCode}")]
        public async Task<IActionResult> GetAccount(string AccountCode)
        {
            var account = await _unitOfWork.Repository<Account, string>().GetByIdAsync(AccountCode);

            if (account == null)
                return NotFound();

            return Ok(account);
        }


        [HttpGet("accountCode")]
        public async Task<IActionResult> GetAccountbyid(string AccountCode)
    {
        var customer = await _unitOfWork.Repository<Account, string>().GetByIdAsync(AccountCode);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Repository<Account, int>().AddAsync(account);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAccount), new { accountCode = account.AccountCode }, account);
        }


        [HttpPut("{accountCode}")]
        public async Task<IActionResult> UpdateAccount(string accountCode, [FromBody] Account account)
        {
            if (accountCode != account.AccountCode)
                return BadRequest();

            _unitOfWork.Repository<Account, int>().UpdateAsync(account);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    }
}
