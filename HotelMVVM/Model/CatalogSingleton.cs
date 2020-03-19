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
        private static CatalogSingleton<T> _pendingCatalog;
        private static CatalogSingleton<T> _singleton;
        public static CatalogSingleton<T> GetCatalog(Action notify, Consumer<T> consumer)
        {
            if (_singleton == null)
            {
                if (_pendingCatalog != null)
                {
                    Loop();
                    return _pendingCatalog;

                    async void Loop()
                    {
                        await Task.Run(() =>
                        {
                            while (_pendingCatalog != null)
                            {
                                Thread.Sleep(100);
                            }
                        });
                        notify();
                    }
                }

                CatalogSingleton<T> catalog = new CatalogSingleton<T>();
                _pendingCatalog = catalog;
                LoadItems();
                return catalog;

                async void LoadItems()
                {
                    List<T> items = await consumer.GetAsync();
                    catalog.RefillCatalog(items);
                    _singleton = catalog;
                    _pendingCatalog = null;
                    notify();
                    catalog._preLoaded = true;
                }
            }

            notify();
            return _singleton;
        }


        private void RefillCatalog(IEnumerable<T> collection)
        {
            _catalog = new ObservableCollection<T>(collection);
        }

        private bool _preLoaded;

        public bool PreLoaded
        {
            get { return _preLoaded; }
        } 
        #endregion

        public ObservableCollection<T> Catalog
        {
            get { return _catalog; }
        }

        private ObservableCollection<T> _catalog;

        private CatalogSingleton()
        {
            _catalog = new ObservableCollection<T>();
            _preLoaded = false;
        }

        public void Add(T item)
        {
            _catalog.Add(item);
        }

        public void Delete(int index)
        {
            _catalog.RemoveAt(index);
        }
    }
}
