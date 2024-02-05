

using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class RoleService
{
    private readonly RoleRepo _roleRepo;

    public RoleService(RoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    /// <summary>
    /// Create a new Role. If role exists, use that one. 
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public RoleEntity CreateRoleEntity(string roleName)
    {
        var roleEntity = _roleRepo.GetOne(x => x.RoleName == roleName);
        roleEntity ??= _roleRepo.Create(new RoleEntity { RoleName = roleName });

        return roleEntity;  
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
    public IEnumerable<RoleEntity> GetAllRoles()
    {
        var roles = _roleRepo.GetAll();
        return roles;
    }

    /// <summary>
    /// Update a role
    /// </summary>
    /// <param name="roleEntity"></param>
    /// <returns></returns>
    public RoleEntity UpdateRole(RoleEntity roleEntity)
    {
        var updatedRoleEntity = _roleRepo.Update(x => x.Id == roleEntity.Id, roleEntity);
        return updatedRoleEntity;
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
