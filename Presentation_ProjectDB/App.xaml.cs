using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation_ProjectDB.ViewModels;
using Presentation_ProjectDB.Views;
using System.Configuration;
using System.Data;
using System.Windows;


namespace Presentation_ProjectDB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost builder;

        public App()
        {
            builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
               services.AddDbContext<UserContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\EC\Datalagring\ProjectDB\Infrastructure\Data\users_db.mdf;Integrated Security=True;Connect Timeout=30"));

                services.AddScoped<RoleRepo>();
                services.AddScoped<AddressRepo>();
                services.AddScoped<UserRepo>();
                services.AddScoped<AuthenticationRepo>();
                services.AddScoped<ContactInformationRepo>();
                services.AddScoped<UserService>();
                services.AddScoped<RoleService>();
                services.AddScoped<AddressService>();
                services.AddScoped<ContactInformationService>();
                services.AddScoped<AuthenticationService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<UserListViewModel>();
                services.AddSingleton<UserListView>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<AddUserViewModel>();
                services.AddTransient<AddUserView>();
                services.AddScoped<DetailsUserViewModel>();
                services.AddScoped<DetailUserView>();
                services.AddSingleton<UpdateUserViewModel>();
                services.AddSingleton<UpdateUserView>();
                services.AddSingleton<RoleView>();
                services.AddSingleton<RoleViewModel>(); 
                services.AddSingleton<UpdateRoleView>();
                services.AddSingleton<UpdateRoleViewModel>();

            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            builder.Start();

            var mainwindow = builder.Services.GetRequiredService<MainWindow>();
            var mainviewmodel = builder.Services.GetRequiredService<MainViewModel>();
            mainviewmodel.CurrentViewModel = builder.Services.GetRequiredService<UserListViewModel>();
            mainwindow.Show();
        }

    }

}
