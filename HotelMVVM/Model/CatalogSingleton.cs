using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelMVVM.Persistency;

namespace HotelMVVM.Model
{
    /// <summary>
    /// A generic CatalogSingleton. It creates one instance of a catalog object consisting of an Observable collection and methods to add and remove from it.
    /// Other classes can access this instance, but they cannot override it or create new catalog instances.
    /// </summary>
    /// <typeparam name="T">The type that determines what the collection in the catalog object will hold.</typeparam>
    public class CatalogSingleton<T>
    {
        #region CatalogGetterOld
        //private static Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        //public static CatalogSingleton<T> GetCatalog()
        //{
        //    if (!_singletons.ContainsKey(typeof(T)))
        //    {
        //        var catalog = new CatalogSingleton<T>();
        //        _singletons.Add(typeof(T), catalog);
        //        catalog._preLoaded = true;
        //    }

        //    return (CatalogSingleton<T>)_singletons[typeof(T)];
        //}

        //private static Dictionary<Type, object> _pendingKeys = new Dictionary<Type, object>();
        //public static CatalogSingleton<T> GetCatalog(Action notify, Consumer<T> consumer) //This should maybe be changed to be more like the observable collection itself, implementing a subscription based system, maybe with an event. 
        //{ //Or a last come, first served principle could be adopted.
        //    if (!_singletons.ContainsKey(typeof(T)))
        //    {
        //        if (_pendingKeys.ContainsKey(typeof(T)))
        //        {
        //            Loop();
        //            return (CatalogSingleton<T>)_pendingKeys[typeof(T)];
        //            async void Loop()
        //            {
        //                await Task.Run(() =>
        //                {
        //                    while (_pendingKeys.ContainsKey(typeof(T)))
        //                    {
        //                        Thread.Sleep(100);
        //                    }
        //                });
        //                notify();
        //            }
        //        }
        //        CatalogSingleton<T> catalog = new CatalogSingleton<T>();
        //        _pendingKeys.Add(typeof(T), catalog);
        //        LoadItems();
        //        return catalog;

        //        async void LoadItems()
        //        {
        //            List<T> items = await consumer.GetAsync();
        //            catalog.RefillCatalog(items);
        //            _singletons.Add(typeof(T), catalog);
        //            _pendingKeys.Remove(typeof(T));
        //            notify();
        //            catalog._preLoaded = true;
        //        }
        //    }

        //    notify();
        //    return (CatalogSingleton<T>)_singletons[typeof(T)];
        //}

        //private void RefillCatalog(IEnumerable<T> collection)
        //{
        //    _catalog = new ObservableCollection<T>(collection);
        //}

        //private bool _preLoaded;

        //public bool PreLoaded
        //{
        //    get { return _preLoaded; }
        //}
        #endregion //New info about static fields in generics means that all of this completely outdated and should be redone.

        #region CatalogGetter
        //private static CatalogSingleton<T> _pendingCatalog;
        //private static CatalogSingleton<T> _singleton;
        //public static CatalogSingleton<T> GetCatalog(Action notify, Consumer<T> consumer)
        //{
        //    if (_singleton == null)
        //    {
        //        if (_pendingCatalog != null)
        //        {
        //            Loop();
        //            return _pendingCatalog;

        //            async void Loop()
        //            {
        //                await Task.Run(() =>
        //                {
        //                    while (_pendingCatalog != null)
        //                    {
        //                        Thread.Sleep(100);
        //                    }
        //                });
        //                notify();
        //            }
        //        }

        //        CatalogSingleton<T> catalog = new CatalogSingleton<T>();
        //        _pendingCatalog = catalog;
        //        LoadItems();
        //        return catalog;

        //        async void LoadItems()
        //        {
        //            List<T> items = await consumer.GetAsync();
        //            catalog.RefillCatalog(items);
        //            _singleton = catalog;
        //            _pendingCatalog = null;
        //            notify();
        //            catalog._preLoaded = true;
        //        }
        //    }

        //    notify();
        //    return _singleton;
        //}


        //private void RefillCatalog(IEnumerable<T> collection)
        //{
        //    _catalog = new ObservableCollection<T>(collection);
        //}

        //private bool _preLoaded;

        //public bool PreLoaded
        //{
        //    get { return _preLoaded; }
        //} 
        #endregion

        #region SimpleGetter(In Use)
        private static CatalogSingleton<T> _singleton = new CatalogSingleton<T>();
        private event Action _finalActions;

        private async void LoadItems()
        {
            Consumer<T> consumer = ConsumerCatalog.GetConsumer<T>();
            List<T> items = await consumer.GetAsync();

            foreach (T item in items) //Auto update so you don't have to subscribe for this to take effect.
            {
                _catalog.Add(item);
            }

            _isLoading = false;
            _finalActions?.Invoke();
            _finalActions = null;
        }

        /// <summary>
        /// This method can be used to update a UI/ViewModel when the catalog is finished loading items from a Database.
        /// The observable collection will update any sort of ListView by itself, but this method can be used to update other things like a loading bar.
        /// This method should always be used in combination with the IsLoading property so that only if the CatalogSingleton is still loading should Subscribe be called.
        /// </summary>
        /// <param name="finalAction">The action that is invoked once items has been loaded into the Catalog.</param>
        public void Subscribe(Action finalAction)
        {
            _finalActions += finalAction;
        }

        public static CatalogSingleton<T> Singleton
        {
            get { return _singleton; }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
        } 
        #endregion

        private CatalogSingleton()
        {
            _catalog = new ObservableCollection<T>();
            _isLoading = true;
            LoadItems();
        }

        /// <summary>
        /// The observable collection that holds items of the specified type T. The Catalog will be consistent across all classes which use the same type T.
        /// </summary>
        public ObservableCollection<T> Catalog
        {
            get { return _catalog; }
        }

        private ObservableCollection<T> _catalog;

        //private CatalogSingleton() //Original constructor.
        //{
        //    _catalog = new ObservableCollection<T>();
        //    _preLoaded = false;
        //}

        /// <summary>
        /// A method to add items to the Catalog.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            _catalog.Add(item);
        }

        /// <summary>
        /// A method to delete items from the Catalog.
        /// </summary>
        /// <param name="index">The item to delete</param>
        public void Delete(int index)
        {
            _catalog.RemoveAt(index);
        }
    }
}
