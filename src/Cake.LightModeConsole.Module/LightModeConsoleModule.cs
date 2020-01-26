using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;

[assembly: CakeModule(typeof(Cake.LightModeConsole.Module.LightModeConsoleModule))]
namespace Cake.LightModeConsole.Module
{
    public class LightModeConsoleModule : ICakeModule
    {
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar is null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            registrar.RegisterType<LightModeConsole>().As<IConsole>().Singleton();
            registrar.RegisterType<CakeBuildLog>().As<ICakeLog>().Singleton();
        }
    }
}
