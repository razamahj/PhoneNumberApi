using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneNumberApi.Data;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumberApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneNumbersController : ControllerBase
    {

        //This field holds an instance of the IPhoneNumberService interface
        private readonly IPhoneNumberService _phoneNumberService;

        //This constructor injects an instance of IPhoneNumberService into the controller
        public PhoneNumbersController(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;
        }

        //This method handles HTTP POST requests to create a new phone number and assign it to an account
        [HttpPost("{accountId}")]
        public ActionResult<PhoneNumber> AssignPhoneNumber(int accountId, PhoneNumber phoneNumber)
        {
            try
            {
                //Returns the assigned phone number as an HTTP 201 (Created) response 
                //Along with the URL to retrieve the created resource
                var assignedPhoneNumber = _phoneNumberService.AssignPhoneNumber(accountId, phoneNumber);
                return CreatedAtAction(nameof(GetPhoneNumber), new { id = assignedPhoneNumber.Id }, assignedPhoneNumber);
            }
            catch (ArgumentException ex)
            {
                //Returns an HTTP 404 (Not Found) response with the exception message if the operation fauils due to an invalid argument 
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                //Returns a HTTP 409 (Conflict) response with the exception message if the operation fails due to a conflict
                return Conflict(ex.Message);
            }
        }

        //This method handles HTTP GET requests to rretrieve a phone number by ID 
        [HttpGet("{id}", Name = "GetPhoneNumber")]
        public ActionResult<PhoneNumber> GetPhoneNumber(int id)
        {
            try
            {
                var phoneNumber = _phoneNumberService.GetPhoneNumber(id);
                return Ok(phoneNumber);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //This method handles HTTP Patch requests to assign a phone number to an account
        [HttpPatch("{id}/assign-to-account/{accountId}")]
        public IActionResult AssignPhoneNumberToAccount(int id, int accountId)
        {
            try
            {
                _phoneNumberService.AssignPhoneNumberToAccount(id, accountId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //This method handles HTTP GET requests to retrieve all phone numbers associated with an accout
        [HttpGet("account/{accountId}")]
        public ActionResult<IEnumerable<PhoneNumber>> GetPhoneNumbersForAccount(int accountId)
        {
            try
            {
                var phoneNumbers = _phoneNumberService.GetPhoneNumbersForAccount(accountId);
                return Ok(phoneNumbers);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //This method handles HTTP DELETE requests to delete a phone number by ID
        [HttpDelete("{id")]
        public IActionResult DeletePhoneNumber(int id)
        {
            try
            {
                _phoneNumberService.DeletePhoneNumber(id); 
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Phone Number not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
