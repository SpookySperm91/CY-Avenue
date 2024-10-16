namespace CarlosYulo.backend.monolith;

public interface InterfaceSearch
{
}

public interface ISearchByFullName<T, TVar>
{
    List<T> SearchByFullName(string fullName, TVar? variable, out string message);
}

public interface ISearchById<T, TVar>
{
    T? SearchById(int membershipId, TVar? variable, out string message);
}

public interface ISearchAll<T, TVar>
{
    List<T> SearchAll(TVar type);
}