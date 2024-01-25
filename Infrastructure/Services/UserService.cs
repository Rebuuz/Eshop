

using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Infrastructure.Services;

public class UserService(ContactInformationRepo contactInformationRepo, UserRepo userRepo, RoleRepo roleRepo, AddressRepo addressRepo, AuthenticationRepo authenticationRepo)
{
    private readonly UserRepo _userRepo = userRepo;
    private readonly RoleRepo _roleRepo = roleRepo;
    private readonly AddressRepo _addressRepo = addressRepo;
    private readonly ContactInformationRepo _contactInformationRepo = contactInformationRepo;
    private readonly AuthenticationRepo _authenticationRepo = authenticationRepo;

    public bool CreateUser(User user)
    {
        if (!_userRepo.Exists(x => x.Email == user.Email))
        {

            //var roleEntity = _roleRepo.GetOne(x => x.RoleName == user.RoleName);
            //roleEntity ??= _roleRepo.Create(new RoleEntity { RoleName = user.RoleName });

            var roleEntity = new RoleEntity
            {
                RoleName = user.RoleName

            };

            var roleResult = _roleRepo.Create(roleEntity);

            var addressEntity = new AddressEntity
            {
                StreetName = user.StreetName,
                City = user.City,
                PostalCode = user.PostalCode,

            };

            var addressResult = _addressRepo.Create(addressEntity);

            var userEntity = new UserEntity
            {
                Email = user.Email,
                AddressId = addressResult.Id,
                RoleId = roleEntity.Id

            };

            var userResult = _userRepo.Create(userEntity);

            var contactInformationEntity = new ContactInformationEntity
            {
                UserId = userResult.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,

            };

            var contactInformaitonResult = _contactInformationRepo.Create(contactInformationEntity);

            var authenticationEntity = new AuthenticationEntity
            {
                UserId = userResult.Id,
                UserName = user.UserName,
                Password = user.Password,
            };

            var authenticationResult = _authenticationRepo.Create(authenticationEntity);


            if (authenticationResult != null)
                return true;

        }
        return false;
    }

    public IEnumerable<User> GetAllUsers()
    {
        var result = _userRepo.GetAll();
        var users = new List<User>();
        foreach (var item in result)
            users.Add(new User
            {
                Email = item.Email,
                FirstName = item.ContactInformation.FirstName,
                LastName = item.ContactInformation.LastName,
                PhoneNumber = item.ContactInformation.PhoneNumber,
                StreetName = item.Address.StreetName,
                City = item.Address.City,
                RoleName = item.Role.RoleName,

            });
        return users;
    }
}
