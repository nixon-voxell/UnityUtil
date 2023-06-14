namespace Voxell.Util.Interface
{
    public interface IDefault<T>
    where T : unmanaged
    {
        T Default();
    }
}
