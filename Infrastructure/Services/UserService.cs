

using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(UserRepo userRepo, RoleService roleService, AddressService addressService, ContactInformationService contactInformationService, AuthenticationService authenticationService)
{
    private readonly UserRepo _userRepo = userRepo;
    private readonly RoleService _roleService = roleService;
    private readonly AddressService _addressService = addressService;
    private readonly ContactInformationService _contactInformationService = contactInformationService;
    private readonly AuthenticationService _authenticationService = authenticationService;



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
    public UserEntity UpdateUser(UserEntity userEntity)
    {
        var updatedUserEntity = _userRepo.Update(x => x.Email == userEntity.Email, userEntity);
        return updatedUserEntity;
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id"></param>
    public bool DeleteRole(Guid id)
    {
        _userRepo.Delete(x => x.Id == id);
        return true;
    }



    //public IEnumerable<UserDto> GetAllUsers()
    //{
    //    var result = _userRepo.GetAll();
    //    var users = new List<UserDto>();
    //    foreach (var item in result)
    //        users.Add(new UserDto
    //        {
    //            Email = item.Email,
    //            FirstName = item.ContactInformation.FirstName,
    //            LastName = item.ContactInformation.LastName,
    //            PhoneNumber = item.ContactInformation.PhoneNumber,
    //            StreetName = item.Address.StreetName,
    //            City = item.Address.City,
    //            RoleName = item.Role.RoleName,

    //        });
    //    return users;
    //}
}
