namespace SchoolApp.IdentityProvider.Application.Validations;

public static class GenericValidation
{
    public static void IsNotNegativeValue(string fieldName, int value)
    {
        if (value < 0)
            throw new FormatException($"{fieldName} must be positve");
    }

    public static void IsNotNegativeValue(string fieldName, decimal value)
    {
        if (value < 0)
            throw new FormatException($"{fieldName} must be positve");
    }
}
