using ReportingTool.Data.Exceptions;

namespace ReportingTool.Data.Utils
{
    public static class ObjectCheck<TEntity> where TEntity : class
    {
        public static void EntityCheck(TEntity entity, string message = null)
        {
            if (entity == null)
            {
                if (message == null)
                    message = ErrorConstants.NotFound;

                throw new NotFoundException(message);
            }
        }

        public static void PrimaryKeyCheck(object primaryKey, string message = null)
        {
            if (primaryKey == null || primaryKey.ToString() == "0")
            {
                if (message == null)
                    message = ErrorConstants.PrimaryKeyNullError;
                throw new NotFoundException(message);
            }
        }
    }
}
