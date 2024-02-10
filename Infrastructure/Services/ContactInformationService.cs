

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

    public async Task<ContactInformationDto> CreateContactInformationAsync(string firstname, string lastname, string? phonenumber, Guid userId)
    {
        try
        {
            var result = await _contactInformationRepo.GetOneAsync(x => x.FirstName == firstname && x.LastName == lastname && x.PhoneNumber == phonenumber && x.UserId == userId );
            result ??= await _contactInformationRepo.CreateAsync(new ContactInformationEntity { FirstName = firstname, LastName = lastname, PhoneNumber = phonenumber, UserId = userId });

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
    public ContactInformationEntity UpdateContactInformation(ContactInformationEntity contactInformationEntity)
    {
        var updatedcontactInformationEntity = _contactInformationRepo.Update(x => x.UserId == contactInformationEntity.UserId, contactInformationEntity);
        return contactInformationEntity;
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




