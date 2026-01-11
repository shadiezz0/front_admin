using Coder.Application.IServices;
using Coder.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            {
                services.AddAutoMapper(Assembly.GetExecutingAssembly());

                //  HTTP Client for Authentication Service 
                services.AddHttpClient<IAuthenticationService, AuthenticationService>();

                //  Current User Service (scoped - one per request)
                services.AddScoped<ICurrentUserService, CurrentUserService>();

                // HTTP Context Accessor ( for CurrentUserService)
                services.AddHttpContextAccessor();

                // Business Logic Services
                services.AddScoped<ICodeTypeService, CodeTypeService>();
                services.AddScoped<ICodeAttributeTypeService, CodeAttributeTypeService>();
                services.AddScoped<ICodeAttributeMainService, CodeAttributeMainService>();
                services.AddScoped<ICodeAttributeDetailsService, CodeAttributeDetailsService>();
                services.AddScoped<ICodeTypeSettingService, CodeTypeSettingService>();
                services.AddScoped<ICodeTypeSequenceService, CodeTypeSequenceService>();
                services.AddScoped<ICodeService, CodeService>();

                return services;
            }
        }
    }
}
 