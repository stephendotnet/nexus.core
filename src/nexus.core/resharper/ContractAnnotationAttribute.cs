/* MIT License

Copyright (c) 2016 JetBrains http://www.jetbrains.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System;

namespace nexus.core.resharper
{
   /// <summary>
   /// Describes dependency between method input and output.
   /// </summary>
   /// <syntax>
   /// <p>Function Definition Table syntax:</p>
   /// <list>
   ///    <item>FDT      ::= FDTRow [;FDTRow]*</item>
   ///    <item>FDTRow   ::= Input =&gt; Output | Output &lt;= Input</item>
   ///    <item>Input    ::= ParameterName: Value [, Input]*</item>
   ///    <item>Output   ::= [ParameterName: Value]* {halt|stop|void|nothing|Value}</item>
   ///    <item>Value    ::= true | false | null | notnull | canbenull</item>
   /// </list>
   /// If method has single input parameter, its name could be omitted.<br />
   /// Using <c>halt</c> (or <c>void</c>/<c>nothing</c>, which is the same) for method output
   /// means that the methos doesn't return normally (throws or terminates the process).<br />
   /// Value <c>canbenull</c> is only applicable for output parameters.<br />
   /// You can use multiple <c>[ContractAnnotation]</c> for each FDT row, or use single attribute
   /// with rows separated by semicolon. There is no notion of order rows, all rows are checked
   /// for applicability and applied per each program state tracked by R# analysis.<br />
   /// </syntax>
   /// <examples>
   ///    <list>
   ///       <item>
   ///          <code>
   /// [ContractAnnotation("=&gt; halt")]
   /// public void TerminationMethod()
   /// </code>
   ///       </item>
   ///       <item>
   ///          <code>
   /// [ContractAnnotation("halt &lt;= condition: false")]
   /// public void Assert(bool condition, string text) // regular assertion method
   /// </code>
   ///       </item>
   ///       <item>
   ///          <code>
   /// [ContractAnnotation("s:null =&gt; true")]
   /// public bool IsNullOrEmpty(string s) // string.IsNullOrEmpty()
   /// </code>
   ///       </item>
   ///       <item>
   ///          <code>
   /// // A method that returns null if the parameter is null,
   /// // and not null if the parameter is not null
   /// [ContractAnnotation("null =&gt; null; notnull =&gt; notnull")]
   /// public object Transform(object data) 
   /// </code>
   ///       </item>
   ///       <item>
   ///          <code>
   /// [ContractAnnotation("=&gt; true, result: notnull; =&gt; false, result: null")]
   /// public bool TryParse(string s, out Person result)
   /// </code>
   ///       </item>
   ///    </list>
   /// </examples>
   [AttributeUsage( AttributeTargets.Method, AllowMultiple = true )]
   internal sealed class ContractAnnotationAttribute : Attribute
   {
      public ContractAnnotationAttribute( [NotNull] String contract )
         : this( contract, false )
      {
      }

      public ContractAnnotationAttribute( [NotNull] String contract, Boolean forceFullStates )
      {
         Contract = contract;
         ForceFullStates = forceFullStates;
      }

      [NotNull]
      public String Contract { get; }

      public Boolean ForceFullStates { get; }
   }
}