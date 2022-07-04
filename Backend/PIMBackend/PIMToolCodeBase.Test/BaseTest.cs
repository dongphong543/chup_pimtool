using Autofac;
using Effort;
using NUnit.Framework;
using PIMToolCodeBase.Configurations;
using PIMToolCodeBase.Database;

namespace PIMToolCodeBase.Test
{
    public abstract class BaseTest
    {
        protected IContainer Container;
        protected PimContext Context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // In memory test only
            Context = new PimContext(DbConnectionFactory.CreateTransient());

            // register the autofac
            Container = AutofacConfigurations.Register(ExtendRegister);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Context?.Dispose();
            Container?.Dispose();
        }

        private void ExtendRegister(ContainerBuilder builder)
        {
            builder.RegisterInstance(Context).As<PimContext>();
        }
    }
}