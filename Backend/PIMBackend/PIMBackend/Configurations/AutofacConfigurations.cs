using System;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using PIMBackend.Database;
using PIMBackend.Repositories;
using PIMBackend.Repositories.Imp;
using PIMBackend.Services;
using PIMBackend.Services.Imp;

namespace PIMBackend.Configurations
{
    public static class AutofacConfigurations
    {
        public static IContainer Register(Action<ContainerBuilder> extendRegister = null)
        {
            var builder = new ContainerBuilder();

            // for all controller in app
            builder.RegisterApiControllers(typeof(Program).Assembly);

            // Services register
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().InstancePerLifetimeScope();
            builder.RegisterType<GroupService>().As<IGroupService>().InstancePerLifetimeScope();

            // Repositories register
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>().InstancePerLifetimeScope();

            // Context
            builder.RegisterType<PIMContext>().AsSelf().InstancePerLifetimeScope();

            // mapper
            builder.RegisterInstance(MapperFactory.GetMapper()).As<IMapper>().SingleInstance();

            // extend register
            extendRegister?.Invoke(builder);

            return builder.Build();
        }
    }
}