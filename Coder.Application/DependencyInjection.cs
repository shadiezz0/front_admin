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
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
