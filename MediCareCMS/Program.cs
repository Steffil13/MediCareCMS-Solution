using MediCareCMS.Repository;
using MediCareCMS.Service;

namespace MediCareCMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ?? Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // ? Add session
            builder.Services.AddHttpContextAccessor(); // Required for session

            // ?? Register Repositories and Services (DI)
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<IDoctorRepository>(provider => new DoctorRepository(connectionString));
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            // ?? Build app
            var app = builder.Build();

            // ?? Middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // ? Enable session

            app.UseAuthorization();

            // ?? Default route ? Login
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
