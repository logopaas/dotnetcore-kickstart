using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NAFCore.Common.Attributes.Settings;
using NAFCore.Common.Types.Config;
using NAFCore.Common.Utils.Extensions;
using NAFCore.DAL.Resource;

namespace LogoPaasSampleApp.Settings
{
    /// <summary>
    /// Class SampleAppDbSettings.
    /// Implements the <see cref="NAFCore.Common.Types.Config.IConfigValidation" />
    /// </summary>
    /// <seealso cref="NAFCore.Common.Types.Config.IConfigValidation" />
    [NDefaultContextOnly]
    [NDisplayName("DBSettings", "DBSettings", typeof(ResourceEnum))]
    [NDescription("DBSettingsDesc", "DBSettingsDesc", typeof(ResourceEnum))]
    public class SampleAppDbSettings : IConfigValidation
    {
        /// <summary>
        /// Gets or sets a value indicating whether [migrate database].
        /// </summary>
        /// <value><c>true</c> if [migrate database]; otherwise, <c>false</c>.</value>
        [NDisplayName("MigrateDatabase", "MigrateDatabase", typeof(ResourceEnum))]
        [NDescription("MigrateDatabaseDesc", "MigrateDatabaseDesc", typeof(ResourceEnum))]
        public bool MigrateDatabase { get; set; } = true;

        /// <summary>
        /// Gets or sets the size of the pool.
        /// </summary>
        /// <value>The size of the pool.</value>
        [NDisplayName("PoolSize", "PoolSize", typeof(ResourceEnum))]
        [NDescription("PoolSizeDesc", "PoolSizeDesc", typeof(ResourceEnum))]
        public int PoolSize { get; set; } = 200;

        /// <summary>
        /// Gets or sets the maximum retry count.
        /// </summary>
        /// <value>The maximum retry count.</value>
        [NDisplayName("MaxRetryCount", "MaxRetryCount", typeof(ResourceEnum))]
        [NDescription("MaxRetryCountDesc", "MaxRetryCountDesc", typeof(ResourceEnum))]
        public int MaxRetryCount { get; set; } = 5;

        /// <summary>
        /// Gets or sets the maximum retry delay.
        /// </summary>
        /// <value>The maximum retry delay.</value>
        [NDisplayName("MaxRetryDelay", "MaxRetryDelay", typeof(ResourceEnum))]
        [NDescription("MaxRetryDelayDesc", "MaxRetryDelayDesc", typeof(ResourceEnum))]
        public int MaxRetryDelay { get; set; } = 30;

        /// <summary>
        /// Gets or sets a value indicating whether [allow data loss on migration].
        /// </summary>
        /// <value><c>true</c> if [allow data loss on migration]; otherwise, <c>false</c>.</value>
        [NDisplayName("AllowDataLossOnMigration", "AllowDataLossOnMigration", typeof(ResourceEnum))]
        [NDescription("AllowDataLossOnMigrationDesc", "AllowDataLossOnMigrationDesc", typeof(ResourceEnum))]
        public bool AllowDataLossOnMigration { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleAppDbSettings"/> class.
        /// </summary>
        public SampleAppDbSettings()
        {

        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return false;
        }
    }
}
