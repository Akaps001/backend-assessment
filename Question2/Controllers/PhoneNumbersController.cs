using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using PhoneLookupApi.Dtos;
using PhoneLookupApi.Services;

namespace PhoneLookupApi.Controllers;

[ApiController]
[Route("api/phone-numbers")]
public partial class PhoneNumbersController(IPhoneNumberService phoneNumberService) : ControllerBase
{
    /// <summary>Detects the country of a phone number and returns the country and its operators.</summary>
    /// <param name="phoneNumber">International format without spaces, e.g. 2348033432323 or +2348033432323.</param>
    [HttpGet("{phoneNumber}")]
    [ProducesResponseType<PhoneNumberLookupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string phoneNumber)
    {
        var normalized = phoneNumber.Trim().TrimStart('+');

        if (!PhoneNumberRegex().IsMatch(normalized))
        {
            return Problem(
                detail: "Phone number must contain 7 to 15 digits, optionally prefixed with '+'.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await phoneNumberService.LookupAsync(normalized);

        if (result is null)
        {
            return Problem(
                detail: $"No country found for phone number '{normalized}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(result);
    }

    // E.164: maximum of 15 digits.
    [GeneratedRegex(@"^\d{7,15}$")]
    private static partial Regex PhoneNumberRegex();
}
