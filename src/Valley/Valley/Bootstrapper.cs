/*
   Copyright 2014 Keith R. Bielaczyc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
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