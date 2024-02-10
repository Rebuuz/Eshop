

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


        UserDto = _userService.CurrentUser;
        
    }

    [ObservableProperty]
    private UserDto userDto = new();

    [RelayCommand]
    private void NavigateToList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToDelete(UserDto userDto)
    {

        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
       
        if (result == MessageBoxResult.Yes)
        {
            _userService.Delete(userDto);

            var mainViewModel = _sp.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
        }
       
       
    }

    [RelayCommand]
    private void NavigateToUpdate()
    {
        _userService.CurrentUser = UserDto;

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UpdateUserViewModel>();
    }
}
