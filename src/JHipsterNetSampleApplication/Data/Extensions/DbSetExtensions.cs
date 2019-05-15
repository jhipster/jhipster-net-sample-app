using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JHipsterNetSampleApplication.Data.Extensions {
    public static class DbSetExtensions {
        public static EntityEntry<TEntity> RemoveById<TEntity>(this DbSet<TEntity> receiver, object id)
            where TEntity : class
        {
            var container = Activator.CreateInstance<TEntity>();
            var idProperty = GetKeyProperty(container.GetType());
            idProperty.SetValue(container, id, null);
            receiver.Attach(container);
            return receiver.Remove(container);
        }

        private static PropertyInfo GetKeyProperty(Type type)
        {
            var key = type.GetProperties().FirstOrDefault(p =>
                p.Name.Equals("ID", StringComparison.OrdinalIgnoreCase)
                || p.Name.Equals(type.Name + "ID", StringComparison.OrdinalIgnoreCase));

            if (key != null) return key;
            key = type.GetProperties().FirstOrDefault(p =>
                p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute)));

            if (key != null) return key;

            //https://stackoverflow.com/questions/25141955/entityframework-6-how-to-get-identity-field-with-reflection
            //TODO complete with FluentAPi
            return null;
        }

        public static void RemoveNavigationProperty<TEntity, TOwnerEntity>(this DbSet<TEntity> receiver, TOwnerEntity ownerEntity, object id)
            where TEntity : class
            where TOwnerEntity : class
        {
            bool success;
            var receiverObjects = receiver.ApplyWhere(ownerEntity.GetType().Name + "Id", id, out success);

            if (success) {
                foreach (TEntity receiverObject in receiverObjects) {
                    receiver.Remove(receiverObject);
                }
            }            
        }

        private static IQueryable<T> ApplyWhere<T>(this IQueryable<T> source, string propertyName, object propertyValue, out bool success)
            where T : class
        {
            // 1. Retrieve member access expression
            success = false;
            var mba = PropertyAccessorCache<T>.Get(propertyName);
            if (mba == null) return source;

            // 2. Try converting value to correct type
            object value;
            try {
                value = Convert.ChangeType(propertyValue, mba.ReturnType);
            }
            catch (SystemException ex) when (
                ex is InvalidCastException ||
                ex is FormatException ||
                ex is OverflowException ||
                ex is ArgumentNullException) {
                return source;
            }

            // 3. Construct expression tree
            var eqe = Expression.Equal(
                mba.Body,
                Expression.Constant(value, mba.ReturnType));
            var expression = Expression.Lambda(eqe, mba.Parameters[0]);

            // 4. Construct new query
            success = true;
            MethodCallExpression resultExpression = Expression.Call(
                null,
                GetMethodInfo(Queryable.Where, source, (Expression<Func<T, bool>>)null),
                new Expression[] { source.Expression, Expression.Quote(expression) });
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        private static MethodInfo GetMethodInfo<T1, T2, T3>(
            Func<T1, T2, T3> f,
            T1 unused1,
            T2 unused2)
        {
            return f.Method;
        }
    }   
}
