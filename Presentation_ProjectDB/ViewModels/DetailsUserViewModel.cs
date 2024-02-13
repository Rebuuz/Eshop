

using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Services;
using Infrastructure.Dtos;
using Microsoft.Identity.Client;
using CommunityToolkit.Mvvm.Input;
using System.Printing;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Collections.ObjectModel;

namespace Presentation_ProjectDB.ViewModels;

public partial class DetailsUserViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly UserService _userService;

    public DetailsUserViewModel(IServiceProvider sp, UserService userService)
    {
        _sp = sp;
        _userService = userService;

        Users = new ObservableCollection<UserDto>(_userService.GetAllUsers());

        UserDto = _userService.CurrentUser;
        
    }

    [ObservableProperty]
    private UserDto userDto = new();

    [ObservableProperty]
    private ObservableCollection<UserDto> _users = [];

    /// <summary>
    /// Relay command for list 
    /// </summary>
    [RelayCommand]
    private void NavigateToList()
    {
       
        _userService.CurrentUser = UserDto;

        Users = new ObservableCollection<UserDto>(_userService.GetAllUsers());

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    /// <summary>
    /// Relay command for delete
    /// </summary>
    /// <param name="userDto"></param>

    [RelayCommand]
    private void NavigateToDelete(UserDto userDto)
    {

        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
       
        if (result == MessageBoxResult.Yes)
        {
            _userService.Delete(userDto);

            Users = new ObservableCollection<UserDto>(_userService.GetAllUsers());

            var mainViewModel = _sp.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
        }
       
       
    }

    /// <summary>
    /// Relay command for update
    /// </summary>
    [RelayCommand]
    private void NavigateToUpdate()
    {
        _userService.CurrentUser = UserDto;

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UpdateUserViewModel>();
    }

}
