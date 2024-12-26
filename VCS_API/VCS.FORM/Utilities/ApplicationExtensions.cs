using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace VCS.FORM.Utilities
{
    public static class ApplicationExtensions
    {
        public static IServiceProvider Services 
        { 
            get 
            {
                var app = (App)Application.Current;
                return app.ServiceProvider;
            }
        }
    }
}
