PURPOSE
-------
The purpose of Valley is to build a CalDAV and CardDAV server on top of Microsoft ASP.NET MVC4. My goal is to rebuild the wonderful ideas behind projects like [Radicale](http://radicale.org/) by leveraging WebAPI concepts and tactics. For example, I want to build a WebDAV server that adheres to the original specification, [RFC 4918](https://tools.ietf.org/html/rfc4918), and provides a simple extensibility model to "bolt-on" the CalDAV- and CardDAV-specific logic, [RFC 4791](http://tools.ietf.org/html/rfc4791).  

At the same time, Valley represents what I think is the simplest possible solution that can work for an archetypal kind of modern application: the application must be composed of independent parts, and it must have various well-known layers. 

![Valley_Layering](https://github.com/keithb-/Valley/raw/master/doc/wiki/images/valley-layering.png "Valley Layering")

GENERAL CHARACTERISTICS
-----------------------
Generally speaking, Valley is config-driven and composed so things like storage/persistance components and security/authorization components are bootstrapped. This should help to make the design more flexible and maintainable. I am leveraging [Microsoft Patterns and Practices Unity](http://unity.codeplex.com/) in most of the work so far, but I would expect that other dependency injection containers could be implemented, as needed.

EXTENSION POINTS
----------------
One of the underlying goals for this project is to leverage the [ASP.NET MVC Framework](http://www.asp.net/mvc). This means understanding the pre-defined extension points and building the application to leverage those points. Valley includes implementations for several of these well-defined extension points including delegating handlers and filters. Additionally, the application leaves room for further extension by defining simple interfaces for storage components and policy enforcement (authorization).

![Valley_Message_Handling](https://github.com/keithb-/Valley/raw/master/doc/wiki/images/valley-message-handling.png "Valley Message Handling")

STORAGE COMPONENTS
------------------
In my case, I don't need robust persistence and I prefer working with plain-old objects when the data has little chance of being aggregated, so I chose to build a bare-bones, in-memory virtual file system to store and retrieve entities. I fully expect that other, more mature solutions will eventually be used in place of this home-grown component. However, for the initial design and development this component serves my needs.

Copyright (c) 2014 by Keith R. Bielaczyc. All Right Reserved.

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License for the specific language governing permissions and limitations under the License.