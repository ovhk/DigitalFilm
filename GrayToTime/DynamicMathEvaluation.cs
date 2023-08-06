using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.CSharp;
//using System.CodeDom.Compiler;
//using System.Reflection;

namespace DigitalFilm.Engine
{
    internal class DynamicMathEvaluation
    {
        // Thanks to https://devreminder.wordpress.com/net/net-framework-fundamentals/c-dynamic-math-expression-evaluation/

        /// <summary>
        /// 
        /// </summary>
        private readonly CSharpCodeProvider codeProvider = new CSharpCodeProvider();

        /// <summary>
        /// 
        /// </summary>
        private readonly CompilerParameters compilerParameters = new CompilerParameters
        {
            GenerateExecutable = false,
            GenerateInMemory = false
        };

        /// <summary>
        /// 
        /// </summary>
        private MethodInfo Methinfo = null;

        public bool Compile(string MathExpression)
        {
            try
            {
                string tmpModuleSource = "namespace DigitalFilm.Engine {"
                + "using System;"
                + "class OnTheFlyEvaluation {"
                + "    public static int Eval(int x) {"
                + "          return Convert.ToInt32(" + MathExpression + ");"
                + "     }"
                + "}} ";

                CompilerResults compilerResults = this.codeProvider.CompileAssemblyFromSource(this.compilerParameters, tmpModuleSource);

                if (compilerResults.Errors.Count > 0)
                {
                    //If a compiler error is generated, we will throw an exception because 
                    //the syntax was wrong - again, this is left up to the implementer to verify 
                    //syntax before calling the function.  The calling code could trap this in a 
                    //try loop, and notify a user  
                    //the command was not understood, for example.
                    throw new ArgumentException("Expression cannot be evaluated [" + MathExpression + "]");
                }
                else
                {
                    this.Methinfo = compilerResults.CompiledAssembly.GetType("DigitalFilm.Engine.OnTheFlyEvaluation").GetMethod("Eval");
                    return true;
                }
            }
            catch (Exception Ex)
            {
                throw new ArgumentException("Expression cannot be evaluated, please use a valid C# expression [" + MathExpression + "] -- Ex[" + Ex.Message + "][" + Ex.InnerException + "]");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gray"></param>
        /// <returns></returns>
        public int Eval(int gray)
        {
            if (this.Methinfo != null)
            {
                object[] parametersArray = new object[] { gray };

                return (int) Methinfo.Invoke(null, parametersArray);
            }

            return 0;
        }
    }
}
