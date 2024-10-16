namespace CarlosYulo.backend.monolith;

public interface InterfaceUpdate { }


public interface IUpdateDetails<T, Tvar>
{
    T UpdateDetails(Tvar? details, out string message);
}


public interface IUpdateProfilePicture<T, Tvar>
{
    T UpdateProfilePicture(Tvar entity, string picturePath, out string message);
}