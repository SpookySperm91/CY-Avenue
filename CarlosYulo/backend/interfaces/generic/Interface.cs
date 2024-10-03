namespace CarlosYulo.backend.monolith;

public class Interface
{
}

public interface ICreate<T>
{
    bool Create(T entity);
}
public interface ISearch<T, TVar>
{
    T? SearchById(int id, TVar? variable);
    T? SearchByFullName(string fullName, TVar? variable);
    List<T> SearchAll();
}

public interface IUpdate<T>
{
    bool Update(T entity);
    bool UpdateProfilePicture(T entity, string picture);
}

public interface IDelete<T>
{
    bool Delete(T entity);
    bool DeleteById(int? id);
}

public interface IAttendance<T>
{
    bool CreateAttendance(T entity);
    bool DeleteEmployeeAttendance(T entity, int id, string fullName, DateTime date);
    bool UpdateEmployeeAttendance(T entity, int id, string fullName, DateTime date);
}