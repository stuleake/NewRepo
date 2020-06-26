using Api.FeeCalculator.Core.ViewModels;
using Jint;
using Newtonsoft.Json;

namespace Api.FeeCalculator.Core.Helpers.JSEngine
{
    /// <summary>
    /// A JS Engine to execute javascript
    /// </summary>
    public class JsEngine
    {
        private readonly Engine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsEngine"/> class.
        /// </summary>
        /// <param name="engine">javascript execution engine</param>
        public JsEngine(Engine engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// A Function to execute javascript of rule calculation with jint
        /// </summary>
        /// <param name="functionDefinition">A javascript function definition as a string</param>
        /// <param name="functionExecution">A js function call as a string</param>
        /// <returns>JSResponse model contaning output details</returns>
        public JsResponseModel ExecuteJavascript(string functionDefinition, string functionExecution)
        {
            engine.Execute(functionDefinition);
            var res = engine.Execute(functionExecution).GetCompletionValue().ToString();
            if (res == null)
            {
                return null;
            }
            var response = JsonConvert.DeserializeObject<JsResponseModel>(res);

            return response;
        }
    }
}