namespace Asteroid.Generation
{
    public interface IInstanceLoader
    {
        public T CreateInstance <T> () where T : new();
    }
}