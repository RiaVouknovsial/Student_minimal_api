namespace Student_minimal_api.Auth
{
    public interface IUserRepository
    {
        UserDto GetUser(UserDto userModel);
    }
}
