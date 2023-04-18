namespace Gamesa.Shared;

public static class Helpers
{
    public static string GenerateRandomGameId()
    {
        char[] chars = new char[3];
        for (var i = 0; i < chars.Length; i++)
        {
            chars[i] = (char)Random.Shared.Next('a', 'z');
        }
        return new string(chars);
    }
}
