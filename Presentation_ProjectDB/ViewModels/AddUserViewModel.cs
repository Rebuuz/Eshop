

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation_ProjectDB.ViewModels;

public partial class AddUserViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly UserService _userService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="userService"></param>
    public AddUserViewModel(IServiceProvider sp, UserService userService)
    {
        _sp = sp;
        _userService = userService;
    }

    [ObservableProperty]
    private UserDto user = new UserDto();

    [RelayCommand]
    private async Task AddUserToList(UserDto userDto)
    {
        await _userService.CreateUserAsync(User!);

        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    [RelayCommand]
    private void NavigateToList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }
}
