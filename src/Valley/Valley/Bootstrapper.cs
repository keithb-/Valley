//using System;
//using System.Web.Http;
//using System.Web.Mvc;
//using Microsoft.Practices.Unity;
//using Unity.Mvc4;

//namespace Valley
//{
//    //http://www.devtrends.co.uk/blog/introducing-the-unity.mvc3-nuget-package-to-reconcile-mvc3-unity-and-idisposable
//    //http://stackoverflow.com/questions/9527988/cannot-inject-dependencies-into-asp-net-web-api-controller-using-unity
//    //http://www.strathweb.com/2012/11/asp-net-web-api-and-dependencies-in-request-scope/
//    //http://www.asp.net/web-api/overview/extensibility/using-the-web-api-dependency-resolver
//    public static class Bootstrapper
//    {
//        public static IUnityContainer Initialize()
//        {
//            var container = new UnityContainer();

//            container.AddNewExtension<Storage.UnityExtension>();
//            container.AddNewExtension<MessageHandlers.UnityExtension>();

//            var resolver = new Unity.WebApi.UnityDependencyResolver(container);
//            GlobalConfiguration.Configuration.DependencyResolver = resolver; 

//            return container;
//        }
//    }
//}