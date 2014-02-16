﻿/*
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
using System.Linq;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;

namespace Valley
{
    public class UnityExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<Valley.MessageHandlers.UnityExtension>();

            // Register the constructor with arguments.
            this.Container.RegisterType<IAuthorizationFilter, AuthorizeDistributedHttpAttribute>(
                new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(
                    new ResolvedParameter(typeof(IHttpActionPolicyEvidenceProvider)), 
                    new ResolvedParameter(typeof(IHttpActionPolicyEnforcer))));
        }
    }
}
