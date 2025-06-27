namespace Asteroid.Generation
{
    public class InstanceCreator : IInstanceLoader
    {
        public T CreateInstance<T>() where T : new()
        {
            return new T(); 
        }
    }
}
