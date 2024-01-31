

using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Services;
using System.Collections.ObjectModel;


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

    

}
