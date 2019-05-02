using LogoPaasSampleApp.Dal;
using LogoPaasSampleApp.DIServices.Imp;
using LogoPaasSampleApp.DIServices.Interface;
using LogoPaasSampleApp.Settings;
using LogoPaasSampleApp.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NAFCore.Common.Log2Fluentd;
using NAFCore.Common.Utils.Diagnostics.Logger;
using NAFCore.Common.Utils.Extensions;
using NAFCore.DAL.Core;
using NAFCore.DAL.EF.Core;
using NAFCore.DAL.EF.Extensions;
using NAFCore.DAL.EF.MultiTenancy;
using NAFCore.DAL.EF.Repositories;
using NAFCore.Platform.Services.Hosting.Types;
using NAFCore.Platform.UI.Razor;
using NAFCore.Services.IDM.WebHelper;
using NAFCore.Services.IDM.WebHelper.Settings;
using NAFCore.Settings;
using NAFCore.Settings.UI.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace LogoPaasSampleApp
{
    /// <summary>
    /// Class Startup.
    /// </summary>
    public class Startup : WebHostStartup
    {
        #region ..Privates-Protected..

        private SampleAppSettings _sampleAppSettings;

        [ThreadStatic]

        protected static SampleAppBaseContext _currentContext = null;
        #endregion

        #region ..Constructors..

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
            : base(env, loggerFactory)
        {
        }

        #endregion

        #region ..Overridde Members..

        /// <summary>
        /// client_secret or security secret of the application
        /// </summary>
        /// <returns></returns>
        protected override Guid BuildSecuritySecret() { return new Guid("85a60f5e-ab96-4a0d-acdb-bab275872aaf"); }

        /// <summary>
        /// Enable static file provider
        /// </summary>
        /// <returns></returns>
        protected override bool UseStaticFiles() { return true; }

        /// <summary>
        /// Uses the default route.
        /// </summary>
        /// <returns>Boolean.</returns>
        protected override bool UseDefaultRoute() { return true; }

        /// <summary>
        /// Automatics the register security secret.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool AutoRegisterSecuritySecret() { return false; }
        /// <summary>
        /// Does the configure logger.
        /// </summary>
        protected override void DoConfigureLogger()
        {
            FluentdExtensions.AddFluentdLog(NAFSettings.Current, GetType().GetTypeInfo().Assembly);
        }

        /// <summary>
        /// Configures the MVC json options.
        /// </summary>
        /// <param name="jsonOptions">The json options.</param>
        protected override void ConfigureMVCJsonOptions(MvcJsonOptions jsonOptions)
        {
            jsonOptions.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

            jsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss.fffzzz";            
        }

        protected override void DoAddServices(IServiceCollection services)
        {            
            services.AddIDMWebHelper();

            //
            services.AddRazorViewEngine();
            //
            // add settings UI
            services.AddSettingsUI<SettingsUIPathOptions>();
        }

        protected override void DoUseStartServices(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            // use settings UI
            app.UseSettingsUI("/api/settings/");

            app.UseIDMWebHelper(new WebHelperSettings());            

            try
            {
                if (_sampleAppSettings.DbSettings.MigrateDatabase)
                {
                    var context = app.ApplicationServices.GetService<DbContext>();
                     context.Database.Migrate();
                }
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            }
            catch (Exception ex)
            {
                NLogger.Instance().Error(ex);
            }
        }

        protected override void InitializeApp(IServiceCollection services)
        {
            _sampleAppSettings = NAFSettings.Current.ReadAppSettings<SampleAppSettings>(throwIfNotFound: false) ?? new SampleAppSettings(true);

            // If you have a singleton backend class, inject it as in below
            services.AddSingleton<ISampleDIService>(new SampleDIService(_sampleAppSettings));

            // Add repository classes
            services.Scan(scan => scan.FromAssembliesOf(typeof(Repository<,>)).AddClasses(classes => classes.AssignableTo(typeof(Repository<,>))));

            // Add EF related DI services
            services.AddScoped<DbContext, SampleAppBaseContext>(provider =>
            {
                return ResolveDbContext(provider);
            });

            services.AddSingleton(_sampleAppSettings);            
        }

        private SampleAppBaseContext ResolveDbContext(IServiceProvider provider)
        {
            var tenantSettingsList = TenantHelper.GetTenantAppSettingsList(_sampleAppSettings, TenantHelper.GetCurrentTenantId(_sampleAppSettings));

            if (tenantSettingsList.NotAssigned() || ((JArray)tenantSettingsList).Count == 0)
                throw new Exception($"Belirtilen kiracı için uygulama ayarları yapılmamıştır: { HttpContextExtensions.GetCurrentContextId()}");

            SampleAppBaseContext dbContext = new SampleAppBaseContext();
            var tenantAppSettings = ((JArray)tenantSettingsList)[0];
            string serverAddr = tenantAppSettings["ServerAddress"].ToString();
            int dbType = tenantAppSettings["DBType"].ToInt();
            string dbName = tenantAppSettings["DBName"].ToString();            
            int? port = tenantAppSettings["Port"].ToInt();
            string serverUsername = tenantAppSettings["ServerUsername"].ToString();
            string serverPassword = tenantAppSettings["ServerPassword"].ToString();
            string schemaName = tenantAppSettings["SchemaName"].ToString();            

            if (dbType == (int)DBTypes.MSSQL)
            {
                DbContextOptionsBuilder<MsSqlContext> sqlOptionsBuilder = new DbContextOptionsBuilder<MsSqlContext>();
                GetDbContextOptions(sqlOptionsBuilder, $"Data Source={serverAddr},{port};Initial Catalog={dbName};User Id={serverUsername};Password={serverPassword};", dbType, schemaName);
                dbContext = new MsSqlContext(sqlOptionsBuilder.Options);
            }
            else if (dbType == (int)DBTypes.PostGreSQL)
            {
                DbContextOptionsBuilder<PostgreSqlContext> sqlOptionsBuilder = new DbContextOptionsBuilder<PostgreSqlContext>();
                GetDbContextOptions(sqlOptionsBuilder, $"Host = {serverAddr}; Database = {dbName}; User ID = {serverUsername}; Password = {serverPassword}; Port = {port};Pooling = true;", dbType, schemaName);
                dbContext = new PostgreSqlContext(sqlOptionsBuilder.Options);
            }

            dbContext.SchemaName = schemaName;
            dbContext.DbType = (DBTypes)dbType;

            var dbInitializer = new DbInitializer(dbContext);
            dbInitializer.Initialize();

            return dbContext;            
        }

        private void GetDbContextOptions<TContext>(DbContextOptionsBuilder<TContext> optionsBuilder, string connectionString, int? DBType, string schemaName) where TContext : DbContext
        {
            if (DBType == (int)DBTypes.MSSQL)
            {
                optionsBuilder.UseSqlServer(connectionString,
                 sqlServerOptionsAction: sqlOptions =>
                 {
                     sqlOptions.MigrationsAssembly(typeof(SampleAppSettings).GetTypeInfo().Assembly.GetName().Name);
                     sqlOptions.EnableRetryOnFailure(maxRetryCount: _sampleAppSettings.DbSettings.MaxRetryCount <= 0 ? 5 : _sampleAppSettings.DbSettings.MaxRetryCount, maxRetryDelay: TimeSpan.FromSeconds(_sampleAppSettings.DbSettings.MaxRetryDelay <= 0 ? 30 : _sampleAppSettings.DbSettings.MaxRetryDelay), errorNumbersToAdd: null);
                 }).ReplaceService<IModelCacheKeyFactory, SampleAppDbContextFactory>(); ;
            }
            else if (DBType == (int)DBTypes.PostGreSQL)
            {
                optionsBuilder.UseNpgsql(connectionString, npgSqlOptions =>
                {
                    npgSqlOptions.MigrationsAssembly(typeof(SampleAppSettings).GetTypeInfo().Assembly.GetName().Name);
                    npgSqlOptions.EnableRetryOnFailure(maxRetryCount: _sampleAppSettings.DbSettings.MaxRetryCount <= 0 ? 5 : _sampleAppSettings.DbSettings.MaxRetryCount, maxRetryDelay: TimeSpan.FromSeconds(_sampleAppSettings.DbSettings.MaxRetryDelay <= 0 ? 30 : _sampleAppSettings.DbSettings.MaxRetryDelay), errorCodesToAdd: null);
                }).ReplaceService<IModelCacheKeyFactory, SampleAppDbContextFactory>(); ;
            }
            else
            {
                NLogger.Instance().Error($"DBtype='{DBType}' is not supported  for tenant={ TenantHelper.GetCurrentTenantId(_sampleAppSettings) }");
            }
        }

        /// <summary>
        /// Does the configure diagnostics.
        /// </summary>
        /// <param name="services">The services.</param>
        protected override void DoConfigureDiagnostics(IServiceCollection services)
        {
        }

        /// <summary>
        /// Class WebHelperSettings.
        /// </summary>
        public class WebHelperSettings : IDMWebHelperSettings
        {
            /// <summary>
            /// Gets the name of the application.
            /// </summary>
            /// <value>The name of the application.</value>
            public override string AppName { get { return "LogoPaasSampleApp"; } }
            /// <summary>
            /// Gets the callback path.
            /// </summary>
            /// <value>The callback path.</value>
            public override string CallbackPath { get { return "/home/index"; } }

            public override bool ResolveIDMAddressFromEurekaForBrowserRedirection()
            {
                return false;
            }
        }
        
        #endregion
    }

    public class SampleAppDbContextFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            if (context is SampleAppBaseContext baseContext)
            {
                return (context.GetType(), baseContext.SchemaName);
            }
            return context.GetType();
        }
    }
}
