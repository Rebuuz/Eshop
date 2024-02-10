

using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace Infrastructure.Services;

public class RoleService
{
    private readonly RoleRepo _roleRepo;

    public RoleService(RoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    /// <summary>
    /// The selected role
    /// </summary>
    public RoleDto CurrentRole { get; set; } = null!;

    /// <summary>
    /// Create a new Role. If role exists, use that one. 
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    /// 
    public RoleEntity CreateRoleEntity(string roleName)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == roleName);
        roleEntity ??= _roleRepo.Create(new RoleEntity { RoleName = roleName });

        return roleEntity;
    }
    /// <summary>
    /// Create Role async method
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>

    public async Task<RoleDto> CreateRoleAsync(string roleName)
    {
        try
        {
            var result = await _roleRepo.GetOneAsync(x => x.RoleName == roleName);
            result ??= await _roleRepo.CreateAsync(new RoleEntity { RoleName = roleName });

            return new RoleDto { Id = result.Id, RoleName = result.RoleName };
        }
        catch { }
        return null!;
    }


    /// <summary>
    /// Get RoleName by Rolename
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public RoleEntity GetRoleByRoleName(string roleName)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == roleName);
        return roleEntity;
    }

    /// <summary>
    /// Get Role by Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoleEntity GetRoleByRoleId(int id)
    {
        var roleEntity = _roleRepo.GetOne(x => x.Id == id);
        return roleEntity;
    }

    /// <summary>
    /// Get all roles in a list
    /// </summary>
    /// <returns></returns>
    public IEnumerable<RoleDto> GetAllRoles()
    {
        List<RoleDto> roles = new List<RoleDto>();

        try
        {
            var result = _roleRepo.GetAll();

            if (result != null)
            {
                foreach (var role in result)
                    roles.Add(new RoleDto
                    {
                        Id = role.Id,
                        RoleName = role.RoleName

                    });
            }
            return roles;

        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    /// Update a role async method
    /// </summary>
    /// <param name="roleEntity"></param>
    /// <returns></returns>
    public async Task<RoleDto> UpdateRoleAsync(RoleDto updatedRole)
    {
        try
        {
            var entity = await _roleRepo.GetOneAsync(x => x.Id == updatedRole.Id);
            if (entity != null)
            {
                entity.RoleName = updatedRole.RoleName!;

                var updatedRoleEntity = await _roleRepo.UpdateOneAsync(entity);
                if (updatedRoleEntity != null)
                    return new RoleDto
                    {
                        Id = updatedRoleEntity.Id,
                        RoleName = updatedRoleEntity.RoleName
                    };
            }
        }catch { }
        return null!;
       
       
    }

    //public async Task<RoleEntity> UpdateUser(RoleEntity role)
    //{
    //    var updatedRoleEntity = await _roleRepo.Update(x => x.Id == role.Id, role);
    //    return updatedRoleEntity;
    //}

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id"></param>
    public bool DeleteRole(int id)
    {
        _roleRepo.Delete(x => x.Id == id);
        return true;
    }

}
