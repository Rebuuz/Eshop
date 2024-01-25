using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

                services.AddSingleton<MainWindow>();
                

            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            builder.Start();

            var mainWindow = builder.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

    }

}
