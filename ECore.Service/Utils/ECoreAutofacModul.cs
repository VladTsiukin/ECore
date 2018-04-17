using Autofac;
using ECore.Service.CardsServices;
using ECore.Service.CardsServices.service;
using System.Reflection;

namespace ECore.Service.Utils
{
    public class ECoreAutofacModul : Autofac.Module
    {
        /// <summary>
        /// This registers all classes with their interface which 'EndsWith("Service")'.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(CRUDService<>))
                .As(typeof(ICRUDService<>))
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(CardsService).Assembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(CardService).Assembly)
               .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(
            //   GetType().GetTypeInfo().Assembly)
            //   .Where(c => c.Name.EndsWith("Service"))
            //   .AsImplementedInterfaces();
        }


    }
}
