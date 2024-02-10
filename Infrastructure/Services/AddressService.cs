

using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AddressService
{
    private readonly AddressRepo _addressRepo;

    public AddressService(AddressRepo addressRepo)
    {
        _addressRepo = addressRepo;
    }

    /// <summary>
    /// Create a new addresss, if the address already exists, use that one 
    /// </summary>
    /// <param name="streetName"></param>
    /// <param name="city"></param>
    /// <param name="postalCode"></param>
    /// <returns></returns>
    public AddressEntity CreateAddressEntity(string streetName, string city, string postalCode)
    {
        var addressEntity = _addressRepo.GetOne(x => x.StreetName == streetName && x.City == city && x.PostalCode == postalCode);
        addressEntity ??= _addressRepo.Create(new AddressEntity { StreetName = streetName, City = city, PostalCode = postalCode  });

        return addressEntity;
    }

    public async Task<AddressDto> CreateAddressAsync(string streetName, string city, string postalCode)
    {
        try
        {
            var result = await _addressRepo.GetOneAsync(x => x.StreetName == streetName && x.City == city && x.PostalCode == postalCode);
            result ??= await _addressRepo.CreateAsync(new AddressEntity { StreetName = streetName, City = city, PostalCode = postalCode });

            return new AddressDto { Id = result.Id, StreetName = result.StreetName, City = result.City, PostalCode = result.PostalCode };
        }
        catch { }
        return null!;
    }

    /// <summary>
    /// Get Address by streetname
    /// </summary>
    /// <param name="streetName"></param>
    /// <returns></returns>
    public AddressEntity GetAddressByStreetName(string streetName)
    {
        var addressEntity = _addressRepo.GetOne(x => x.StreetName == streetName);
        return addressEntity;
    }

    /// <summary>
    /// Get Address by city
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    public AddressEntity GetAddressByCity(string city)
    {
        var addressEntity = _addressRepo.GetOne(x => x.City == city);
        return addressEntity;
    }

    /// <summary>
    /// Get Address by Postal code
    /// </summary>
    /// <param name="postalCode"></param>
    /// <returns></returns>
    public AddressEntity GetAddressByPostalCode(string postalCode)
    {
        var addressEntity = _addressRepo.GetOne(x => x.PostalCode == postalCode);
        return addressEntity;
    }

    /// <summary>
    /// Get address by Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AddressEntity GetAddressById(int id)
    {
        var addressEntity = _addressRepo.GetOne(x => x.Id == id);
        return addressEntity;
    }

    /// <summary>
    /// Get all addresses in a list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<AddressEntity> GetAllAddresses()
    {
        var addresses = _addressRepo.GetAll();
        return addresses;
    }

    /// <summary>
    /// Update an address
    /// </summary>
    /// <param name="addressEntity"></param>
    /// <returns></returns>
    public AddressEntity UpdateAddress(AddressEntity addressEntity)
    {
        var updatedAddressEntity = _addressRepo.Update(x => x.Id == addressEntity.Id, addressEntity);
        return updatedAddressEntity;
    }

    public async Task<AddressDto> UpdateAddressAsync(AddressDto updatedAddress)
    {
        try
        {
            var entity = await _addressRepo.GetOneAsync(x => x.Id == updatedAddress.Id);
            if (entity != null)
            {
                entity.StreetName = updatedAddress.StreetName!;
                entity.City = updatedAddress.City!;
                entity.PostalCode = updatedAddress.PostalCode!;

                var updatedAddressEntity = await _addressRepo.UpdateOneAsync(entity);
                if (updatedAddressEntity != null)
                    return new AddressDto
                    {
                        Id = updatedAddressEntity.Id,
                        StreetName = updatedAddressEntity.StreetName,
                        City = updatedAddressEntity.City,
                        PostalCode = updatedAddressEntity.PostalCode,
                    };
            }
        }
        catch { }
        return null!;


    }

    /// <summary>
    /// Delete an address
    /// </summary>
    /// <param name="id"></param>
    public void DeleteRole(int id)
    {
        _addressRepo.Delete(x => x.Id == id);
    }
}
