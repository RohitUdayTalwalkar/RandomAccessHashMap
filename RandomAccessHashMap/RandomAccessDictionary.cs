using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAccessHashMap
{
    public class RandomAccessDictionary<KeyType,ObjectType>
    {
        /// <summary>
        /// Represents a collection similar to System.Collections.Generic.Dictionary
        /// but also contains a function to get random elements.
        /// </summary>
        /// <param name="startCapacity">Initial capacity, must be greater than 10000.</param>
        /// <param name="randomizer">Random number generator.</param>
        public RandomAccessDictionary(int startCapacity, Random randomizer)
        {
            ThrowExceptionIfStartCapacityIsTooSmall(startCapacity);
            _data = new Dictionary<KeyType, ObjectType>(startCapacity);
            _randomList = new LinkedList<KeyType>();
            _randomizer = randomizer;
        }

        /// <summary>
        /// Returns the element stored with the specified key.
        /// Throws a KeyNotFoundException if key is missing.
        /// </summary>
        /// <param name="key">Key used to store object.</param>
        /// <returns>Object stored in collection.</returns>
        public ObjectType Get(KeyType key)
        {
            return _data[key];
        }

        /// <summary>
        /// Removes the object stored with the specied key from the collection.
        /// </summary>
        /// <param name="key">Key used to store object.</param>
        /// <returns>True if object was removed.</returns>
        public bool Delete(KeyType key)
        {
            return _data.Remove(key);
        }

        /// <summary>
        /// Stores the object in the collection with the specified key.
        /// Throws an ArgumentException if key already exists in collection.
        /// </summary>
        /// <param name="key">Key used to store object.</param>
        /// <param name="item">Item to store in collection.</param>
        public void Put(KeyType key, ObjectType item)
        {
            _data.Add(key, item);
            AddKeyAtRandomToStartOrEndOfList(key);
        }

        private void AddKeyAtRandomToStartOrEndOfList(KeyType key)
        {
            if (_randomizer.Next(10) > 5)
            {
                _randomList.AddFirst(key);
            }
            else
            {
               _randomList.AddLast(key);
            }
        }

        /// <summary>
        /// Returns a randomly selected object from the collection.
        /// </summary>
        /// <returns>Random item from collection.</returns>
        public ObjectType GetRandom()
        {
            if (_data.ContainsKey(_randomList.First.Value))
            {
                return ReturnFirstValueAndMoveToEndOfList();
            }

            _randomList.RemoveFirst();
            return _data[_data.Keys.First()];
        }

        private ObjectType ReturnFirstValueAndMoveToEndOfList()
        {
            var randomValue = _data[_randomList.First.Value];
            _randomList.AddLast(_randomList.First.Value);
            _randomList.RemoveFirst();
            return randomValue;
        }

        #region "Private Data Members"
        
        private const int minCapacity = 10000;
        private Random _randomizer;

        private LinkedList<KeyType> _randomList;

        /// <summary>
        /// Store the collection in .NET's Dictionary class.
        /// This member is marked internal to allow the unit-tests access.
        /// </summary>
        internal Dictionary<KeyType, ObjectType> _data; 
       
        #endregion

        #region "Private Functions"

        private static void ThrowExceptionIfStartCapacityIsTooSmall(int startCapacity)
        {
            if (minCapacity > startCapacity)
                throw new ArgumentException(
                    String.Format("Please use a start capacity larger than {0}", minCapacity));
        }

        #endregion
    }
}
