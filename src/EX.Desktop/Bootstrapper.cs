using System;
using EX.Desktop.Forms;
using EX.Desktop.Services;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace EX.Desktop
{
    public static class Bootstrapper
    {
        private static readonly Container _container;

        static Bootstrapper()
        {
            //instance
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //service
            _container.RegisterSingleton<MembershipService>();
            _container.RegisterSingleton<SessionService>();
            _container.RegisterSingleton<SpeedService>();
            _container.RegisterSingleton<ExamService>();
            _container.RegisterSingleton<StreamService>();

            //form
            _container.Register<NetworkForm>(Lifestyle.Scoped);
            _container.Register<LoginForm>(Lifestyle.Scoped);
            _container.Register<AudioForm>(Lifestyle.Scoped);
            _container.Register<CameraForm>(Lifestyle.Scoped);
            _container.Register<SettingsForm>(Lifestyle.Scoped);
            _container.Register<StreamForm>(Lifestyle.Scoped);
            _container.Register<FinishForm>(Lifestyle.Scoped);

            _container.Verify();
        }

        public static void ExecuteScope<T>(Action<T> action) where T : class
        {
            using (AsyncScopedLifestyle.BeginScope(_container))
            {
                var t = GetService<T>();

                action(t);
            }
        }

        public static T GetService<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}