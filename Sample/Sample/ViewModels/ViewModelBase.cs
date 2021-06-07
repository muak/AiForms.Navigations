using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AiForms.PrismNavigationEx;
using Prism;
using Prism.Mvvm;
using Prism.Navigation;

namespace Sample.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IInitializeAsync, INavigationAware, IActiveAware, IDestructible
    {       
        protected INavigationServiceEx NavigationService { get; }

        static ConcurrentDictionary<Type, List<string>> PropertiesCache = new ConcurrentDictionary<Type, List<string>>();

#pragma warning disable 67
        public event EventHandler IsActiveChanged;
#pragma warning disable 67

        Action _onActiveFirst;

        private bool _IsActive;
        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
                if (value)
                {
                    _onActiveFirst?.Invoke();
                    _onActiveFirst = null;
                    OnActive();
                }
                else
                {
                    OnInActive();
                }
            }
        }

        public ViewModelBase()
        {
            _onActiveFirst = OnActiveFirst;
        }
        public ViewModelBase(INavigationServiceEx navigationService) : this()
        {
            NavigationService = navigationService;
        }


        public virtual async Task InitializeAsync(INavigationParameters parameters)
        {
            await Initialize(parameters);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                OnComeBack();
            }
            else
            {
                PageLoaded();
            }
        }

        public virtual void Destroy()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is destoryed");
        }

        internal virtual async Task Initialize(INavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is initialized");
        }

        internal virtual void PageLoaded()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is loaded");
        }

        internal virtual void OnComeBack()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is comeback");
        }

        internal virtual void OnActiveFirst()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is active first");
        }

        internal virtual void OnActive()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is active");            
        }

        internal virtual void OnInActive()
        {
            System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} is inactive");
        }

        protected void RaisePropertyChangedAll()
        {
            var props = PropertiesCache.GetOrAdd(this.GetType(), CreatePropertiesList);
            foreach (var p in props)
            {
                RaisePropertyChanged(p);
            }
        }        

        List<string> CreatePropertiesList(Type t)
        {
            return
                t.GetRuntimeProperties()
                 .Where(x => !x.Name.StartsWith("_", StringComparison.Ordinal))
                 .Select(x => x.Name).ToList();
        }
    }
}