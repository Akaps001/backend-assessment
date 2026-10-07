using Microsoft.EntityFrameworkCore;
using PhoneLookupApi.Data;
using PhoneLookupApi.Dtos;

namespace PhoneLookupApi.Services;

public interface IPhoneNumberService
{
    Task<PhoneNumberLookupResponse?> LookupAsync(string phoneNumber);
}

public class PhoneNumberService(AppDbContext context) : IPhoneNumberService
{
    public async Task<PhoneNumberLookupResponse?> LookupAsync(string phoneNumber)
    {
        var countries = await context.Countries
            .AsNoTracking()
            .Include(c => c.CountryDetails)
            .ToListAsync();

        // Longest matching prefix wins, so codes like "1" and "1876" would resolve correctly.
        var country = countries
            .Where(c => phoneNumber.StartsWith(c.CountryCode, StringComparison.Ordinal))
            .MaxBy(c => c.CountryCode.Length);

        if (country is null)
        {
            return null;
        }

        var details = country.CountryDetails
            .OrderBy(d => d.Id)
            .Select(d => new CountryDetailDto(d.Operator, d.OperatorCode))
            .ToList();

        return new PhoneNumberLookupResponse(
            phoneNumber,
            new CountryDto(country.CountryCode, country.Name, country.CountryIso, details));
    }
}
