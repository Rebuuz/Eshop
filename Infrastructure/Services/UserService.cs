

using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Migrations;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
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
                var roleEntity = _roleService.CreateRoleEntity(user.RoleName);
                var addressEntity = _addressService.CreateAddressEntity(user.StreetName, user.City, user.PostalCode);

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
    //public UserEntity UpdateUser()
    //{
    //    var updatedUserEntity = _userRepo.Update(x => x.Email == x.Email);
    //    return updatedUserEntity;
    //}

    //public async Task<UserDto> UpdateUserAsync(UserDto user)
    //{
    //    var updatedUserEntity = await _userRepo.UpdateAsync(x => x.Email == user.Email, user);
    //    return updatedUserEntity;
    //}

    ///Uppdaterar inte till databasen... 
    //public async Task<UserDto> UpdateUserAsync(UserDto updatedUser)
    //{
    //    try
    //    {
    //        var userEntity = new UserEntity { Email = updatedUser.Email };
    //        var updatedUserEntity = await _userRepo.UpdateAsync(x => x.Email == updatedUser.Email, userEntity);

    //        if (updatedUserEntity != null)
    //        {
    //            var user = new UserDto
    //            {
    //                FirstName = updatedUser.FirstName,
    //                LastName = updatedUser.LastName,
    //                Email = updatedUser.Email,
    //                PhoneNumber = updatedUser.PhoneNumber,
    //                RoleName = updatedUser.RoleName,
    //                StreetName = updatedUser.StreetName,
    //                City = updatedUser.City,
    //                PostalCode = updatedUser.PostalCode,
    //                UserName = updatedUser.UserName,
    //                Password = updatedUser.Password,
    //            };
    //            return user;
    //        }
    //    }catch (Exception ex)
    //    {
    //        Debug.WriteLine("ERROR ::" + ex.Message );  
    //    }
    //    return null!;
    //}

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

    public async Task<UserEntity> UpdateUserAsync(UserDto updatedUser)
    {
        try
        {

            var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
            // kontrollera om email finns och om den finns mot annat id .
            //  if (existingUserEntity.email != updatedUser.emil && _service.Exists (x => x.email == updatedUser.email && x.id != updatedUser.id)
            // { return false }
            // ... fortsätterden med update

             



            if (existingUserEntity != null)
            {
                existingUserEntity.Role.RoleName = updatedUser.RoleName;

                existingUserEntity.Address.StreetName = updatedUser.StreetName;
                existingUserEntity.Address.PostalCode = updatedUser.PostalCode;
                existingUserEntity.Address.City = updatedUser.City;

                existingUserEntity.ContactInformation.FirstName = updatedUser.FirstName;
                existingUserEntity.ContactInformation.LastName = updatedUser.LastName;
                existingUserEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber;


                existingUserEntity.Authentication.UserName = updatedUser.UserName;
                existingUserEntity.Authentication.Password = updatedUser.Password;


                existingUserEntity.Email = updatedUser.Email;
                existingUserEntity.Id = existingUserEntity.Id; 

                var result = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingUserEntity);
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR :: " + ex.Message);
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
