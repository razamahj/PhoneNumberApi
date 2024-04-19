using Microsoft.AspNetCore.Mvc;
using PhoneNumberApi.Data;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumberApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        //This field holds an instance of the IAccountService interface
        private readonly IAccountService _accountService;

        //This constructor injects an instance of IAccountService into the controller
        public AccountsController(IAccountService accountService)
        {

            _accountService = accountService;
        }

        //This method handles HTTP GET requests to retrieve a list of accounts 
        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            //Returning the retrieved accounts as a HTTP 200 (OK) response
            var accounts = _accountService.GetAccounts();
            return Ok(accounts);
        }

        //This method handles the HTTP POST requests to create a new account
        [HttpPost]
        public ActionResult<Account> CreateAccount(Account account)
        {
            //Calls the CreateAccount method of the injected IAccountService instance,
            //passing the recieved account object as a parameter
            //Returns the newly created account as an HTTP 201 (Created) response
            //Along with the URL to retrieve the created resource
            var createdAccount = _accountService.CreateAccount(account);
            return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.Id }, createdAccount);
        }

        //This method handles the HTTP PATCH requests to toggle the status of an account.
        [HttpPatch("{id}/toggle-status")]
        public IActionResult ToggleAccountStatus(int id)
        {
            try
            {
                //Return a HTTP 204 (No Content) response if successful
                _accountService.ToggleAccountStatus(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public ActionResult<Account> GetAccount(int id)
        {
            try
            {
                var account = _accountService.GetAccount(id);
                return Ok(account);
            }
            catch (KeyNotFoundException)
            {
                //Returns a HTTP 404(Not Found) response if the specified account ID is not found
                return NotFound();
            }
        }
    }
}
