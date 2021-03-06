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

using System.Collections.Generic;
using Microsoft.Python.Analysis.Documents;
using Microsoft.Python.Analysis.Types;
using Microsoft.Python.Analysis.Values;
using Microsoft.Python.Core.Text;
using Microsoft.Python.Parsing.Ast;

namespace Microsoft.Python.Analysis {
    /// <summary>
    /// Represents analysis of the Python module.
    /// </summary>
    public interface IDocumentAnalysis {
        /// <summary>
        /// Analyzed document.
        /// </summary>
        IDocument Document { get; }

        /// <summary>
        /// Version of the analysis. Usually matches document version,
        /// but can be lower when document or its dependencies were
        /// updated since.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// AST that was used in the analysis.
        /// </summary>
        PythonAst Ast { get; }

        /// <summary>
        /// Document/module global scope.
        /// </summary>
        IGlobalScope GlobalScope { get; }

        /// <summary>
        /// Module top-level variables
        /// </summary>
        IVariableCollection TopLevelVariables { get; }

        /// <summary>
        /// All module members from all scopes.
        /// </summary>
        IEnumerable<IVariable> AllVariables { get; }

        /// <summary>
        /// Evaluates a given expression and returns a list of members which
        /// exist in the expression.
        /// 
        /// If the expression is an empty string returns all available members
        /// at that location.
        /// </summary>
        /// <param name="location">The location in the file where the expression should be evaluated.</param>
        IEnumerable<IPythonType> GetMembers(SourceLocation location);

        /// <summary>
        /// Evaluates the given expression in at the provided line number and returns the values
        /// that the expression can evaluate to.
        /// </summary>
        /// <param name="location">The location in the file where the expression should be evaluated.</param>
        IEnumerable<IPythonType> GetValues(SourceLocation location);

        /// <summary>
        /// Gets information about the available signatures for the given expression.
        /// </summary>
        /// <param name="location">The location in the file.</param>
        IEnumerable<IPythonFunctionOverload> GetSignatures(SourceLocation location);

        /// <summary>
        /// Gets the available items at the given location. This includes
        /// built-in variables, global variables, and locals.
        /// </summary>
        /// <param name="location">
        /// The location in the file where the available members should be looked up.
        /// </param>
        IEnumerable<IPythonType> GetAllAvailableItems(SourceLocation location);
    }
}
