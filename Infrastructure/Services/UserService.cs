

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

    //public async Task<UserDto> CreateUserAsync(UserDto user)
    //{
    //    try
    //    {
    //        var result = await _userRepo.GetOneAsync(x => x.Email == user.Email);
    //        if (result == null)
    //        {
    //            var roleEntity = await _roleService.CreateRoleAsync(user.RoleName);
    //            var addressEntity = await _addressService.CreateAddressAsync(user.StreetName, user.City, user.PostalCode);

    //            var userEntity = new UserEntity
    //            {
    //                Email = user.Email,
    //                RoleId = roleEntity.Id,
    //                AddressId = addressEntity.Id,
    //            };

    //            result = await _userRepo.CreateAsync(userEntity);
    //            if (result != null)
    //                return new UserDto
    //                {
    //                    Id = result.Id,
    //                    Email = result.Email,
    //                    RoleDto = new RoleDto
    //                    {
    //                        Id = result.Role.Id,
    //                        RoleName = result.Role.RoleName,
    //                    },
    //                    AddressDto = new AddressDto
    //                    {
    //                        Id = result.Address.Id,
    //                        StreetName = result.Address.StreetName,
    //                        City = result.Address.City,
    //                        PostalCode = result.Address.PostalCode,
    //                    }
    //                };

    //            if (result != null)
    //            {
    //                var contactInformationEntity = _contactInformationService.CreateContactInformationEntity(user.FirstName, user.LastName, result.Id, user.PhoneNumber);
    //                var authenticationEntity = _authenticationService.CreateAuthenticationEntity(user.UserName, user.Password, result.Id);


    //            }



    //        }
    //    }
    //    catch { }
    //    return null!;

    //}


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

    //public async Task<UserEntity> UpdateUserAsync(UserDto updatedUser)
    //{
    //    try
    //    {
    //        var entity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
    //        if (entity != null)
    //        {
    //            var roleEntity = await _roleService.CreateRoleAsync(updatedUser.RoleName);
    //            var addressEntity = await _addressService.CreateAddressAsync(updatedUser.StreetName,
    //                updatedUser.City,
    //                updatedUser.PostalCode);

    //            var userEntity = new UserEntity
    //            {
    //                Email = updatedUser.Email,
    //                RoleId = roleEntity.Id,
    //                AddressId = addressEntity.Id,
    //            };

    //            var userResult = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, entity);

    //            if (userResult != null)
    //            {
    //                var contactInformationEntity = _contactInformationService.CreateContactInformationEntity(updatedUser.FirstName, updatedUser.LastName, userResult.Id, updatedUser.PhoneNumber);
    //                var authenticationEntity = _authenticationService.CreateAuthenticationEntity(updatedUser.UserName, updatedUser.Password, userResult.Id);
    //            }

    //            return userResult!;


    //        }

    //    }catch { }
    //    return null!;
    //}

    public async Task<UserEntity> UpdateUserAsync(UserDto updatedUser)
    {
        try
        {
            var existingEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
            if (existingEntity != null)
            {
                var roleEntity = await _roleService.CreateRoleAsync(existingEntity.Role.RoleName = updatedUser.RoleName);
                var addressEntity = await _addressService.CreateAddressAsync(existingEntity.Address.StreetName = updatedUser.StreetName,
                    existingEntity.Address.City = updatedUser.City,
                    existingEntity.Address.PostalCode = updatedUser.PostalCode);

                var userEntity = new UserEntity
                {
                    Email = updatedUser.Email,
                    RoleId = roleEntity.Id,
                    AddressId = addressEntity.Id,
                };

                var userResult = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingEntity);

                if (userResult != null)
                {
                    var contactInformationEntity = _contactInformationService.CreateContactInformationEntity(
                        existingEntity.ContactInformation.FirstName = updatedUser.FirstName, 
                        existingEntity.ContactInformation.LastName = updatedUser.LastName, 
                        existingEntity.Id, 
                        existingEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber);
                    var authenticationEntity = _authenticationService.CreateAuthenticationEntity(
                        existingEntity.Authentication.UserName = updatedUser.UserName, 
                        existingEntity.Authentication.Password = updatedUser.Password, 
                        existingEntity.Id);
                }

                return userResult!;


            }

        }
        catch { }
        return null!;
    }

    //public async Task<UserEntity> UpdateUserAsync(UserDto updatedUser)
    //{
    //    try
    //    {

    //        var existingUserEntity = await _userRepo.GetOneAsync(x => x.Id == updatedUser.Id);
    //        // kontrollera om email finns och om den finns mot annat id .
    //        //if (existingUserEntity.Email != updatedUser.Email && _userRepo.Exists(x => x.Email == updatedUser.Email && x.Id != updatedUser.Id))
    //        //{ return false; }
    //        // ... fortsätterden med update



    //        if (existingUserEntity != null)
    //        {
    //            existingUserEntity.Role.RoleName = updatedUser.RoleName;

    //            existingUserEntity.Address.StreetName = updatedUser.StreetName;
    //            existingUserEntity.Address.PostalCode = updatedUser.PostalCode;
    //            existingUserEntity.Address.City = updatedUser.City;

    //            existingUserEntity.ContactInformation.FirstName = updatedUser.FirstName;
    //            existingUserEntity.ContactInformation.LastName = updatedUser.LastName;
    //            existingUserEntity.ContactInformation.PhoneNumber = updatedUser.PhoneNumber;


    //            existingUserEntity.Authentication.UserName = updatedUser.UserName;
    //            existingUserEntity.Authentication.Password = updatedUser.Password;


    //            existingUserEntity.Email = updatedUser.Email;
    //            existingUserEntity.Id = existingUserEntity.Id;

    //            var result = await _userRepo.UpdateAsync(x => x.Id == updatedUser.Id, existingUserEntity);
    //            return result;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine("ERROR :: " + ex.Message);
    //    }

    //    return null!;
    //}

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
