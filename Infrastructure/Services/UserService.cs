

using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Update;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace Infrastructure.Services;

public class UserService(UserContext userContext, UserRepo userRepo, RoleService roleService, AddressService addressService, ContactInformationService contactInformationService, AuthenticationService authenticationService)
{
    private readonly UserRepo _userRepo = userRepo;
    private readonly RoleService _roleService = roleService;
    private readonly AddressService _addressService = addressService;
    private readonly ContactInformationService _contactInformationService = contactInformationService;
    private readonly AuthenticationService _authenticationService = authenticationService;
    private readonly UserContext _userContext = userContext;

   

    public UserDto CurrentUser { get; set; } = null!;

    private readonly Dictionary<Guid, UserEntity> _userCache = new Dictionary<Guid, UserEntity>();

    /// <summary>
    /// Creating a user, checking if Role and Address exists before
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool CreateUser(UserDto user)
    {
        try
        {
            if (!_userRepo.Exists(x => x.Email == user.Email))
            {
                var roleEntity = _roleService.CreateRoleAsync(user.RoleName);
                var addressEntity = _addressService.CreateAddressAsync(user.StreetName, user.City, user.PostalCode);

                var userEntity = new UserEntity
                {
                    Email = user.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id,
                };
                
                var userResult = _userRepo.Create(userEntity);

                if (userResult != null)
                {
                    var contactInformationEntity = _contactInformationService.CreateContactInformationEntity(user.FirstName, user.LastName, userResult.Id, user.PhoneNumber);
                    var authenticationEntity =  _authenticationService.CreateAuthenticationEntity(user.UserName, user.Password, userResult.Id);
                }

                return true;

            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return false;

    }

    /// <summary>
    /// Create a user async
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<UserEntity> CreateUserAsync(UserDto user)
    {
        try
        {
            if (!_userRepo.Exists(x => x.Email == user.Email))
            {
                var roleEntity = await _roleService.CreateRoleAsync(user.RoleName);
                var addressEntity = await _addressService.CreateAddressAsync(user.StreetName, user.City, user.PostalCode);

                var userEntity = new UserEntity
                {
                    Email = user.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id,
                };

                var userResult = await _userRepo.CreateAsync(userEntity);

                if (userResult != null)
                {
                    var contactInformationEntity = _contactInformationService.CreateContactInformationEntity(user.FirstName, user.LastName, userResult.Id, user.PhoneNumber);
                    var authenticationEntity = _authenticationService.CreateAuthenticationEntity(user.UserName, user.Password, userResult.Id);
                }


                return userResult!;

            }
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;

    }

    private void ClearUserCache(Guid userId)
    {
        if (_userCache.ContainsKey(userId))
        {
            _userCache.Remove(userId);
        }
    }

    /// <summary>
    /// Update a user async
    /// </summary>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    public async Task<UserEntity> UpdateUserAsync(UserDto updatedUser)
    {
        try
        {
            var existingEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
            if (existingEntity != null)
            {
                var roleEntity = await _roleService.CreateRoleAsync(updatedUser.RoleName);
                var addressEntity = await _addressService.CreateAddressAsync(updatedUser.StreetName,
                    updatedUser.City,
                    updatedUser.PostalCode);

                var userEntity = new UserEntity
                {
                    Id = updatedUser.Id,    
                    Email = updatedUser.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id,
                };

                var userResult = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, userEntity);

                    await _contactInformationService.UpdateContactInformation(updatedUser.Id, updatedUser.FirstName, updatedUser.LastName, updatedUser.PhoneNumber);
                    await _authenticationService.UpdateAuth(updatedUser.Id, updatedUser.UserName, updatedUser.Password);

                
                ClearUserCache(updatedUser.Id);
                return userResult;


            }

        }
        catch { }
        return null!;
    }

 



    /// <summary>
    /// Get User by Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public UserEntity GetUserByEmail(string email)
    {
        var userEntity = _userRepo.GetOne(x => x.Email == email);
        return userEntity;
    }

    /// <summary>
    /// Get user by Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UserEntity GetUserById(Guid id)
    {
        var userEntity = _userRepo.GetOne(x => x.Id == id);
        return userEntity;
    }

    /// <summary>
    /// Get all users in a list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UserDto> GetAllUsers()
    {
        ////var users = new List<UserDto>();
        List<UserDto> users = new List<UserDto>();
        try
        {
            var result = _userRepo.GetAll();

            if (result != null)
            {
                foreach (var user in result)
                     users.Add(new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.ContactInformation.FirstName,
                        LastName = user.ContactInformation.LastName,
                        Email = user.Email,
                        PhoneNumber = user.ContactInformation.PhoneNumber,
                        RoleName = user.Role.RoleName,
                        StreetName = user.Address.StreetName,
                        City = user.Address.City,
                        PostalCode = user.Address.PostalCode,
                        UserName = user.Authentication.UserName,
                        Password = user.Authentication.Password,
                    });
            }
            return users;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
        


    }

    /// <summary>
    /// Update a user by email instead of guid Id 
    /// </summary>
    /// <param name="userEntity"></param>
    /// <returns></returns>
   

    public async Task<UserEntity> UpdateEmailAsync(UserDto updatedUser)
    {
        try
        {
            var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
            if (existingUserEntity != null)
            {
                existingUserEntity.Email = updatedUser.Email;
                await _userContext.SaveChangesAsync();
                return await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingUserEntity);
                
            }
        }
        catch
        {

        }
        return null!;
    }

    

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id"></param>
    public bool Delete(UserDto userDto)
    {
        _userRepo.Delete(x => x.Email == userDto.Email);
        return true;
    }

}
