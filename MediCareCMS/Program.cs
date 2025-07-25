using MediCareCMS.Repositories;
using MediCareCMS.Repository;
using MediCareCMS.Service;
using MediCareCMS.Services;
using Rotativa.AspNetCore;

namespace MediCareCMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ? Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // ? Add session
            builder.Services.AddHttpContextAccessor(); // Required for session

            // ? Register Repositories and Services (DI)
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();

            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();

            builder.Services.AddScoped<ILabRepository, LabRepository>();
            builder.Services.AddScoped<ILabService, LabService>();

            builder.Services.AddScoped<IPharmacistService, PharmacistService>();
            builder.Services.AddScoped<IPharmacistRepository, PharmacistRepository>();

            // ? Build app
            var app = builder.Build();

            // ? Configure Rotativa (PDF generator) � IMPORTANT
            //RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

            // ? Middleware pipeline
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

            // ? Default route ? Login
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
