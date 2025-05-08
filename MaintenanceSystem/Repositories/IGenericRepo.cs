using System.Linq.Expressions;

public interface IGenericRepo
{
    Task<List<T>> GetAll<T>() where T : class;
    Task<T> GetById<T>(int id) where T : class;
    Task<T> Insert<T>(T entity) where T : class;
    Task<T> Update<T>(T entity) where T : class;
    Task<bool> Delete<T>(T entity) where T : class;
    Task<bool> DeleteById<T>(int id) where T : class;
    Task<bool> DeleteByExpression<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<List<T>> FindByExpression<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<T> FindModel<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<List<T>> ProcedureToList<T>(string procedureName, string[]  parameterName, object[] parameterValue) where T : class;
    Task<int> ExecuteProcedureAsync(string procedureName, string[] parameterName, object[] parameterValue);
}
