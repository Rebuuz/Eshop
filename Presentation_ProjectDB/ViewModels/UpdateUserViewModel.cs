﻿

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation_ProjectDB.ViewModels;

public partial class UpdateUserViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly UserService _userService;

    public UpdateUserViewModel(IServiceProvider sp, UserService userService)
    {
        _sp = sp;
        _userService = userService;

        User = _userService.CurrentUser;
    }

    [ObservableProperty]
    private UserDto user = new UserDto();

    /// <summary>
    /// Forces an update of the list after updating user
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task UpdateUser()
    {
        await _userService.UpdateUserAsync(User);

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<DetailsUserViewModel>();
    }

    /// <summary>
    /// Navigates to user list
    /// </summary>
    [RelayCommand]
    private void NavigateToList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    /// <summary>
    /// Navigate to update
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task UpdateUserEmail()
    {
        await _userService.UpdateEmailAsync(User);

        var mainVewModel = _sp.GetRequiredService<MainViewModel>();
        mainVewModel.CurrentViewModel = _sp.GetRequiredService<DetailsUserViewModel>();
    }


}
