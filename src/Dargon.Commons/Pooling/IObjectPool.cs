namespace Dargon.Commons.Pooling {
   public interface IObjectPool<T> {
      string Name { get; }
      int Count { get; }

      /// <summary>
      /// Takes an object from the object pool.
      /// The object need not be returned, as the object pool will instantiate
      /// new instances if existing instances do not exist.
      /// </summary>
      /// <returns></returns>
      T TakeObject();

      /// <summary>
      /// Returns an object to the object pool.
      /// </summary>
      /// <param name="item"></param>
      void ReturnObject(T item);
   }
}
