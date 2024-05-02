namespace Student_minimal_api.Auth
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDto user);
    }
}
