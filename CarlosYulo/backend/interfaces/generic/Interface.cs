namespace CarlosYulo.backend.monolith;

public class Interface
{
    
}

public interface ICreate<T>
{
    bool Create(T entity);   
}

public interface ISearch<T>
{
    T? SearchById(int id, string? gender);
    T? SearchByFullName(string fullName, string? gender);
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
    bool DeleteById(int id);
}
