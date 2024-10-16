namespace CarlosYulo.backend.monolith;

public interface InterfaceDelete { }


public interface IDeleteById
{
    bool DeleteById(int id, out string message);
}

public interface IDeleteByEntity<T>
{
    bool DeleteEntity(T entity, out string message);
}
