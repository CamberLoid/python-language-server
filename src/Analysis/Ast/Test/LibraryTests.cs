﻿// Copyright(c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY
// IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABILITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Python.Analysis.Tests.FluentAssertions;
using Microsoft.Python.Analysis.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace Microsoft.Python.Analysis.Tests {
    [TestClass]
    public class LibraryTests : AnalysisTestBase {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
            => TestEnvironmentImpl.TestInitialize($"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}");

        [TestCleanup]
        public void Cleanup() => TestEnvironmentImpl.TestCleanup();

        [TestMethod, Priority(0)]
        public async Task Random() {
            var analysis = await GetAnalysisAsync("from random import *");

            foreach (var fnName in new[] { @"seed", @"randrange", @"gauss" }) {
                var v = analysis.Should().HaveVariable(fnName).Which;
                v.Should().HaveType(BuiltinTypeId.Function);
                v.Value.GetPythonType().Documentation.Should().NotBeNullOrEmpty();
            }
        }

        [TestMethod, Priority(0)]
        public async Task Datetime() {
            var analysis = await GetAnalysisAsync("import datetime");

            var module = analysis.Should().HaveVariable("datetime")
                .Which.Should().HaveType<IPythonModule>().Which;
            module.Name.Should().Be("datetime");

            var dt = module.Should().HaveMember<IPythonClassType>("datetime").Which;

            dt.Should().HaveReadOnlyProperty("day").And.HaveMethod("now")
                .Which.Should().BeClassMethod().And.HaveSingleOverload()
                .Which.Should().HaveReturnType()
                .Which.Should().HaveMembers(
                    @"astimezone", @"isocalendar", @"resolution", @"fromordinal", @"fromtimestamp",
                    @"min", @"max", @"date", @"utcnow", "combine", "replace", "second");
        }
    }
}
