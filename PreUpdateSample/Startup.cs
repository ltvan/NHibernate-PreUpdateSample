using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace PreUpdateSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var persistenceModel = AutoMap.Assemblies(
                new AutoMappingConfiguration(),
                Enumerable.Empty<Assembly>());

            persistenceModel
                .Conventions.AddAssembly(Assembly.GetExecutingAssembly())
                .Conventions.Add(
                    ConventionBuilder.Id.Always(x => x.Column("Id")),
                    ForeignKey.EndsWith("Id"),
                    ConventionBuilder.Property.When(
                        x => x.Expect(y => y.ReadOnly),
                        x => x.Access.CamelCaseField(FluentNHibernate.Conventions.Inspections.CamelCasePrefix.Underscore)
                    ),
                    DynamicInsert.AlwaysTrue(),
                    DynamicUpdate.AlwaysTrue(),
                    OptimisticLock.Is(x => x.Version()))
                .UseOverridesFromAssemblyOf<Startup>()
                .AddEntityAssembly(typeof(Startup).Assembly)
                .AddMappingsFromAssemblyOf<Startup>();

            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(this.Configuration.GetConnectionString("Default")))
                .ExposeConfiguration(config =>
                {
                    config.SetInterceptor(new AbpNHibernateInterceptor());
                    new SchemaValidator(config).Validate();
                })
                .Mappings(
                        m =>
                        {
                            m.AutoMappings.Add(persistenceModel);
                        })
                .BuildSessionFactory();

            services.AddSingleton<ISessionFactory>(sessionFactory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}
