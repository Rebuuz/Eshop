
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class AuthenticationService
{
    private readonly AuthenticationRepo _authenticationRepo;
    

    public AuthenticationService(AuthenticationRepo authenticationRepo)
    {
        _authenticationRepo = authenticationRepo;
        
    }

    /// <summary>
    /// Create an authentication for user by checking if there is a password or username already
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passWord"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AuthenticationEntity CreateAuthenticationEntity(string userName, string passWord, Guid userId)
    {
        try
        {

                var authenticationEntity = new AuthenticationEntity()
                {
                    UserId = userId,
                    UserName = userName,
                    Password = passWord,
                };
                var result = _authenticationRepo.Create(authenticationEntity);
                if (result != null)
                    return result;
           
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;


    }

    /// <summary>
    /// Create auth async method
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<AuthenticationDto> CreateAuthenticationAsync(string username, string password, Guid userId)
    {
        try
        {
            var result = await _authenticationRepo.GetOneAsync(x => x.UserName == username && x.Password == password && x.UserId == userId );
            result ??= await _authenticationRepo.CreateAsync(new AuthenticationEntity { Password = password, UserName = username, UserId = userId });

            return new AuthenticationDto { UserName = result.UserName, Password = result.Password, UserId = result.UserId};
        }
        catch { }
        return null!;
    }

    /// <summary>
    /// Get authentication by looking for userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AuthenticationEntity GetAuthenticationByUserId(Guid userId)
    {
        var authenticationEntity = _authenticationRepo.GetOne(x => x.UserId == userId);
        return authenticationEntity;
    }



    /// <summary>
    /// Get all authentications in a list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<AuthenticationEntity> GetAllAuthentications()
    {
        var authentications = _authenticationRepo.GetAll();
        return authentications;
    }

    /// <summary>
    /// Update username and/or password
    /// </summary>
    /// <param name="authenticationEntity"></param>
    /// <returns></returns>
   
    public async Task<bool> UpdateAuth(Guid userId, string UserName, string Password)
    {
        try
        {
            var newAuth = await _authenticationRepo.UpdateOneAsync(new AuthenticationEntity
            {
                UserId = userId,
                UserName = UserName,
                Password = Password
            });
            return newAuth != null;
        }
        catch (Exception)
        {

           
        }
        return false;
    }

    /// <summary>
    /// Delete password/username
    /// </summary>
    /// <param name="id"></param>
    public void DeleteAuthentication(Guid userId)
    {
        _authenticationRepo.Delete(x => x.UserId == userId);
    }


}
