using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExtensionsLibrary
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Enables the tracking.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="enableEntityTracking">if set to <c>true</c> [enable entity tracking].</param>
        /// <returns>IQueryable</returns>
        public static IQueryable<TEntity> EnableTracking<TEntity>(this IQueryable<TEntity> source, bool enableEntityTracking) where TEntity : class
        {
            return enableEntityTracking ? source.AsTracking() : source.AsNoTracking();
        }

        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

        //public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        //{
        //    if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
        //    {
        //        throw new ArgumentException("Invalid query");
        //    }

        //    var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
        //    var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
        //    var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
        //    var queryModel = parser.GetParsedQuery(query.Expression);
        //    var database = DataBaseField.GetValue(queryCompiler);
        //    var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
        //    var queryCompilationContext = queryCompilationContextFactory.Create(false);
        //    var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
        //    modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
        //    var sql = modelVisitor.Queries.First().ToString();

        //    return sql;
        //}
    }
}