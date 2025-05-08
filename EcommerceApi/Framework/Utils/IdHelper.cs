namespace EcommerceApi.Framework.Utils;
public static class IdHelper
{
    public static Guid GenerateId(string guid = "")
    {
        if (string.IsNullOrEmpty(guid))
        {
            return Guid.NewGuid();
        }

        return Guid.Parse(guid);
    }
}