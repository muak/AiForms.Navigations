using System;
using System.Collections.Generic;
using AiForms.PrismNavigationEx;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Sample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample
{
    public partial class App : PrismApplication
    {
        public App()
        {           
        }

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer)
        {
            InitializeComponent();            
        }

        protected override void OnInitialized()
        {


            //MainPage = new RootPage();
            //MainPage = new NormalTabbedPage();
            //NavigationService.NavigateAsync("MyTabbedPage?createTab=NaviA|PageA&createTab=NaviB|PageB&createTab=NaviC|PageC&createTab=NaviD|PageD&createTab=NaviE|PageE");

            var navi = (PageNavigationServiceEx)Container.Resolve<INavigationServiceEx>(PageNavigationServiceEx.PageNavigationServiceExName);

            MainPage = navi.CreateMainPageTabbedHasNavigation("MyTabbedPage", new List<NavigationPage>
            {
                navi.CreateNavigationPage("NaviA","PageA"),
                navi.CreateNavigationPage("NaviB","PageB"),
                navi.CreateNavigationPage("NaviC","PageC"),
                navi.CreateNavigationPage("NaviD","PageD"),
                navi.CreateNavigationPage("NaviE","PageE"),
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MyTabbedPage>();
            containerRegistry.RegisterForNavigation<NaviA>();
            containerRegistry.RegisterForNavigation<NaviB>();
            containerRegistry.RegisterForNavigation<NaviC>();
            containerRegistry.RegisterForNavigation<NaviD>();
            containerRegistry.RegisterForNavigation<NaviE>();
            containerRegistry.RegisterForNavigation<PageA>();
            containerRegistry.RegisterForNavigation<PageB>();
            containerRegistry.RegisterForNavigation<PageC>();
            containerRegistry.RegisterForNavigation<PageD>();
            containerRegistry.RegisterForNavigation<PageE>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.Register<INavigationServiceEx, PageNavigationServiceEx>();    // VM以外でDIするために必要
            containerRegistry.Register<INavigationServiceEx, PageNavigationServiceEx>(PageNavigationServiceEx.PageNavigationServiceExName);
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {

                INavigationServiceEx navigationService = null;
                switch (view)
                {
                    case Page page:
                        navigationService = Container.CreateNavigationService(page);
                        break;
                    case VisualElement visualElement:
                        if (visualElement.TryGetParentPage(out var attachedPage))
                        {
                            navigationService = Container.CreateNavigationService(attachedPage);
                        }
                        break;
                }

                return Container.Resolve(type, (typeof(INavigationServiceEx), navigationService));
            });
        }
    }
}
