namespace Asteroid.Generation
{
    public interface IInstanceCreator
    {
        public T CreateInstance <T> () where T : new();
    }
}