

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

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

    [ObservableProperty]
    RoleDto _newRole = new();


    [RelayCommand]
    private async Task AddRoleToList()
    {
        if(NewRole.RoleName != null)
        {
            await _roleService.CreateRoleAsync(NewRole.RoleName);

            NewRole = new RoleDto();

            Roles = new ObservableCollection<RoleDto>(_roleService.GetAllRoles());

            var mainViewModel = _sp.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _sp.GetRequiredService<RoleViewModel>();
        }

        
    }

    [RelayCommand]
    private void NavigateToList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]  
    private void NavigateToUpdateRole(RoleDto role)
    {

        _roleService.CurrentRole = role;

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UpdateRoleViewModel>();
    }

    [RelayCommand]
    private void NavigateToDelete(RoleDto role)
    {

        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this role?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            _roleService.DeleteRole(role);

            var mainViewModel = _sp.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _sp.GetRequiredService<RoleViewModel>();
        }

    }

}
