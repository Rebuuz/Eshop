

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation_ProjectDB.ViewModels;

public partial class RoleViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly RoleService _roleService;

    public RoleViewModel(IServiceProvider sp, RoleService roleService)
    {
        _sp = sp;
        _roleService = roleService;

        Roles = new ObservableCollection<RoleDto>(_roleService.GetAllRoles());

        Role = _roleService.CurrentRole;
    }

    [ObservableProperty]
    private ObservableCollection<RoleDto> _roles = new ObservableCollection<RoleDto>();

    [ObservableProperty]
    private RoleDto role = new RoleDto();

    [RelayCommand]
    private async Task AddRoleToList()
    {

        await _roleService.CreateRoleAsync(Role.RoleName!);

        Role = new RoleDto();

        Roles = new ObservableCollection<RoleDto>(_roleService.GetAllRoles());

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<RoleViewModel>();
    }

    [RelayCommand]  
    private async Task UpdateRole()
    {
        await _roleService.UpdateRoleAsync(Role);

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UpdateRoleViewModel>();
    }

}
