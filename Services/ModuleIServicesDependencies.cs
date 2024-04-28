
using Domin.Models;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstracts;
using Services.Implementations;

namespace Core
{
    public static class ModuleIServicesDependencies
    {
        public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IApartmentServices, ApartmentServices>();
            services.AddScoped<IGlobalErrorResponse, GlobalErrorResponse>();
            services.AddScoped<IUploadingMedia, UploadingMedia>();
            services.AddScoped<IRoyalServices, RoyalServices>();
            services.AddScoped<IVideosServices, VideosServices>();
            services.AddScoped<IImagesServices, ImagesServices>();
            services.AddScoped<IViewServices, ViewServices>();
            services.AddScoped<ICommentServices, CommentServices>();
            services.AddScoped<IReactServices, ReactServices>();
            services.AddScoped<iNotifyServices, NotifyServices>();
            services.AddScoped<IUserApartmentsRequestsService, UserApartmentsRequestsService>();
            services.AddScoped<IUsersApartmentsServices, UsersApartmentsServices>();
            services.AddScoped<IPaymentService, PaymentService>();


            return services;
        }
    }
}
