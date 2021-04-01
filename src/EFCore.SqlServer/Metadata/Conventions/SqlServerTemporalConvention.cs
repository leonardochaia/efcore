// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions
{
    /// <summary>
    ///     TODO: add comments
    /// </summary>
    public class SqlServerTemporalConvention : IModelFinalizingConvention
    {
        /// <summary>
        ///     TODO: add comments
        /// </summary>
        public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
        {
            // TODO: is this the right place to do all the tweaks?
            foreach (var rootEntityType in modelBuilder.Metadata.GetEntityTypes().Select(et => et.GetRootType()).Distinct())
            {
                if (rootEntityType[SqlServerAnnotationNames.Temporal] is SqlServerTemporalTableTransientAnnotationValue transientValue)
                //if (rootEntityType.FindAnnotation(SqlServerAnnotationNames.Temporal) is IConventionAnnotation annotation
                //    && annotation.Value is SqlServerTemporalTableTransientAnnotationValue transientValue)
                {
                    var startProperty = rootEntityType.FindProperty(transientValue.PeriodStartPropertyName);
                    var startColumn = startProperty!.GetColumnBaseName();
                    startProperty!.SetValueGenerated(ValueGenerated.OnAddOrUpdate);
                    var endProperty = rootEntityType.FindProperty(transientValue.PeriodEndPropertyName);
                    endProperty!.SetValueGenerated(ValueGenerated.OnAddOrUpdate);
                    var endColumn = endProperty!.GetColumnBaseName();

                    var finalValue = new SqlServerTemporalTableAnnotationValue(startColumn, endColumn, transientValue.HistoryTableName);
                    rootEntityType.SetAnnotation(SqlServerAnnotationNames.Temporal, finalValue);
                }
            }

            foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
            {
                if (entityType != entityType.GetRootType()
                    && entityType.GetRootType()[SqlServerAnnotationNames.Temporal] is SqlServerTemporalTableAnnotationValue rootAnnotationValue
                    //&& entityType.GetRootType().FindAnnotation(SqlServerAnnotationNames.Temporal) is IConventionAnnotation rootAnnotation
                    //&& rootAnnotation.Value is SqlServerTemporalTableAnnotationValue rootAnnotationValue
                    //&& entityType.FindAnnotation(SqlServerAnnotationNames.Temporal) == null)
                    && entityType[SqlServerAnnotationNames.Temporal] != null)
                {
                    entityType.SetAnnotation(SqlServerAnnotationNames.Temporal, rootAnnotationValue);
                }
            }
        }
    }
}
