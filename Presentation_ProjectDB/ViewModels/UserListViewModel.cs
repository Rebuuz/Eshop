

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;


namespace Presentation_ProjectDB.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly UserService _userService;

    public UserListViewModel(IServiceProvider sp, UserService userService)
    {
        _sp = sp;
        _userService = userService;

        
        Users = new ObservableCollection<UserDto>(_userService.GetAllUsers());
    }

    [ObservableProperty]
    private ObservableCollection<UserDto> _users = new ObservableCollection<UserDto>();

    


    [RelayCommand]
    private void NavigateToAddUser()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<AddUserViewModel>();
    }

    [RelayCommand]
    private void NavigateToDetail(UserDto user)
    {
        _userService.CurrentUser = user;

        var mainVewModel = _sp.GetRequiredService<MainViewModel>();
        mainVewModel.CurrentViewModel = _sp.GetRequiredService<DetailsUserViewModel>();
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
}
