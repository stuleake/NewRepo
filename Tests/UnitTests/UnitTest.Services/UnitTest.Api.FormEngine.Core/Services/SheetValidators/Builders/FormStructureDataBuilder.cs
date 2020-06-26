using System.Collections.Generic;
using Api.FormEngine.Core.ViewModels.SheetModels;
using TQ.Data.FormEngine.Schemas.Forms;

namespace UnitTest.Api.FormEngine.Core.Services.SheetValidators.Builders
{
    /// <summary>
    /// Form Structure Data Builder
    /// </summary>
    public class FormStructureDataBuilder
    {
        private readonly FormStructureData formStructureData;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormStructureDataBuilder"/> class.
        /// </summary>
        public FormStructureDataBuilder()
        {
            formStructureData = new FormStructureData
            {
                Displays = new List<Display>
                {
                    new Display
                    {
                        DisplayId = 1,
                        Displays = "1"
                    }
                },
                Functions = new List<Function>
                {
                    new Function
                    {
                        Functions = "function",
                        FunctionsId = 1
                    }
                }
            };
        }

        /// <summary>
        /// Builds an instance of <see cref="FormStructureData"/>.
        /// </summary>
        /// <returns>An instance of <see cref="FormStructureData"/>.</returns>
        public FormStructureData Build()
        {
            return formStructureData;
        }
    }
}
