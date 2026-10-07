namespace PhoneLookupApi.Dtos;

public record PhoneNumberLookupResponse(string Number, CountryDto Country);

public record CountryDto(
    string CountryCode,
    string Name,
    string CountryIso,
    IReadOnlyList<CountryDetailDto> CountryDetails);

public record CountryDetailDto(string Operator, string OperatorCode);
