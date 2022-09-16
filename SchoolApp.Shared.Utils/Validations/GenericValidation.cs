using SchoolApp.Shared.Utils.Enums;

namespace SchoolApp.Shared.Utils.Validations;

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

    public static void ListHaveAtLeastOneItem<TType>(string fieldName, IList<TType> list)
    {
        if (list.Count == 0)
            throw new FormatException($"{fieldName} array must have at least one item");
    }

    public static void CheckOnlyManagerUser(UserTypeEnum userType)
    {
        if (userType != UserTypeEnum.Manager)
            throw new UnauthorizedAccessException("This resource is just to manager users");
    }

    public static void CheckOnlyTeacherAndManagerUser(UserTypeEnum userType)
    {
        if (userType != UserTypeEnum.Teacher && userType != UserTypeEnum.Manager)
            throw new UnauthorizedAccessException("This resource is just to manager or teacher users");
    }
}
