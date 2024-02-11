

using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class ContactInformationService
{
    private readonly ContactInformationRepo _contactInformationRepo;
    

    public ContactInformationService(ContactInformationRepo contactInformationRepo)
    {
        _contactInformationRepo = contactInformationRepo;
        
    }

    /// <summary>
    /// Create an authentication for user by checking if there is a password or username already
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="passWord"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ContactInformationEntity CreateContactInformationEntity(string firstName, string lastName, Guid userId, string? phoneNumber)
    {
        try
        {

            var contactInformationEntity = new ContactInformationEntity()
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };
            var result = _contactInformationRepo.Create(contactInformationEntity);
            if (result != null)
                return result;

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;


    }

    public async Task<ContactInformationDto> CreateContactInformationAsync(string firstName, string lastName, string? phoneNumber, Guid userId)
    {
        try
        {
            var result = await _contactInformationRepo.GetOneAsync(x => x.FirstName == firstName && x.LastName == lastName && x.PhoneNumber == phoneNumber && x.UserId == userId );
            result ??= await _contactInformationRepo.CreateAsync(new ContactInformationEntity { FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, UserId = userId });

            return new ContactInformationDto { FirstName = result.FirstName, LastName = result.LastName, PhoneNumber = result.PhoneNumber, UserId = result.UserId };
        }
        catch { }
        return null!;
    }

    /// <summary>
    /// Get authentication by looking for userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ContactInformationEntity GetContactInformationByUserId(Guid userId)
    {
        var contactInformationEntity = _contactInformationRepo.GetOne(x => x.UserId == userId);
        return contactInformationEntity;
    }



    /// <summary>
    /// Get all authentications in a list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ContactInformationEntity> GetAllContactInformation()
    {
        var contactInformation = _contactInformationRepo.GetAll();
        return contactInformation;
    }

    /// <summary>
    /// Update username and/or password
    /// </summary>
    /// <param name="authenticationEntity"></param>
    /// <returns></returns>
    public async Task<bool> UpdateContactInformation(Guid userId, string FirstName, string LastName, string? PhoneNumber)
    {
        try
        {
            var newContactInformation = await _contactInformationRepo.UpdateOneAsync(new ContactInformationEntity
            {
                UserId = userId,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber
            });
            return newContactInformation != null;
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
    public void DeleteContactInformation(Guid userId)
    {
        _contactInformationRepo.Delete(x => x.UserId == userId);
    }


}




