using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.Web.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();


            var apiAssembly = Assembly.GetExecutingAssembly();  //üzerinde çalıştığın assembly'i al demek. (Zaten api de çalıştığımız için api'yi böyle alıyoruz)
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));  //AppDbContext'in bulunduğu assembly
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile)); //MapProfile'ın bulunduğu assembly


            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            // AsImplementedInterfaces interfaceleri implemente et demek
            // Autofac'de InstancePerLifetimeScope, .net core'da Scope'a karşılık gelir

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces
              ().InstancePerLifetimeScope();

        }
    }
}
