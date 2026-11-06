namespace Asteroid.Generation
{
    public class InstanceCreator : IInstanceCreator
    {
        public T CreateInstance<T>() where T : new()
        {
            return new T(); 
        }
    }
}
