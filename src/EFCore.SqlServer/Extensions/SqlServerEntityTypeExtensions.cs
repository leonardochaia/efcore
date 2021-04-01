// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     Entity type extension methods for SQL Server-specific metadata.
    /// </summary>
    public static class SqlServerEntityTypeExtensions
    {
        /// <summary>
        ///     Returns a value indicating whether the entity type is mapped to a memory-optimized table.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> <see langword="true" /> if the entity type is mapped to a memory-optimized table. </returns>
        public static bool IsMemoryOptimized(this IReadOnlyEntityType entityType)
            => entityType[SqlServerAnnotationNames.MemoryOptimized] as bool? ?? false;

        /// <summary>
        ///     Sets a value indicating whether the entity type is mapped to a memory-optimized table.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <param name="memoryOptimized"> The value to set. </param>
        public static void SetIsMemoryOptimized(this IMutableEntityType entityType, bool memoryOptimized)
            => entityType.SetOrRemoveAnnotation(SqlServerAnnotationNames.MemoryOptimized, memoryOptimized);

        /// <summary>
        ///     Sets a value indicating whether the entity type is mapped to a memory-optimized table.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <param name="memoryOptimized"> The value to set. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        /// <returns> The configured value. </returns>
        public static bool? SetIsMemoryOptimized(
            this IConventionEntityType entityType,
            bool? memoryOptimized,
            bool fromDataAnnotation = false)
        {
            entityType.SetOrRemoveAnnotation(SqlServerAnnotationNames.MemoryOptimized, memoryOptimized, fromDataAnnotation);

            return memoryOptimized;
        }

        /// <summary>
        ///     Gets the configuration source for the memory-optimized setting.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> The configuration source for the memory-optimized setting. </returns>
        public static ConfigurationSource? GetIsMemoryOptimizedConfigurationSource(this IConventionEntityType entityType)
            => entityType.FindAnnotation(SqlServerAnnotationNames.MemoryOptimized)?.GetConfigurationSource();

        /// <summary>
        ///     Returns a value indicating whether the entity type is mapped to a temporal table.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> <see langword="true" /> if the entity type is mapped to a temporal table. </returns>
        public static bool IsTemporal(this IReadOnlyEntityType entityType)
            => entityType[SqlServerAnnotationNames.Temporal] is SqlServerTemporalTableTransientAnnotationValue
            || entityType[SqlServerAnnotationNames.Temporal] is SqlServerTemporalTableAnnotationValue;

        /// <summary>
        ///     Sets a value indicating that the entity type is mapped to a temporal table and relevant mapping configuration.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <param name="temporal"> The value to set. </param>
        public static void SetIsTemporal(this IMutableEntityType entityType, SqlServerTemporalTableTransientAnnotationValue temporal)
            => entityType.SetOrRemoveAnnotation(SqlServerAnnotationNames.Temporal, temporal);

        /// <summary>
        ///     Sets a value indicating that the entity type is mapped to a temporal table and relevant mapping configuration.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <param name="periodStartPropertyName"> A value specifying the property name representing start of the period.</param>
        /// <param name="periodEndPropertyName"> A value specifying the property name representing end of the period.</param>
        /// <param name="historyTableName"> A value specifying the history table name for this entity. Default name will be used if none is specified. </param>
        /// <param name="fromDataAnnotation"> Indicates whether the configuration was specified using a data annotation. </param>
        public static void SetIsTemporal(
            this IConventionEntityType entityType,
            string periodStartPropertyName,
            string periodEndPropertyName,
            string? historyTableName = null,
            bool fromDataAnnotation = false)
        {
            // TODO: maumar - do we need to return value here?
            var value = new SqlServerTemporalTableTransientAnnotationValue(periodStartPropertyName, periodEndPropertyName, historyTableName);
            entityType.SetOrRemoveAnnotation(SqlServerAnnotationNames.Temporal, value, fromDataAnnotation);
        }

        /// <summary>
        ///     Gets the configuration source for the temporal table setting.
        /// </summary>
        /// <param name="entityType"> The entity type. </param>
        /// <returns> The configuration source for the temporal table setting. </returns>
        public static ConfigurationSource? GetIsTemporalConfigurationSource(this IConventionEntityType entityType)
            => entityType.FindAnnotation(SqlServerAnnotationNames.Temporal)?.GetConfigurationSource();
    }
}
