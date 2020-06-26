using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using TQ.Core.Constants;

namespace TQ.Core.Filters
{
    /// <summary>
    /// A class for custom validation of input data
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CustomInputValidateAttribute : ValidationAttribute
    {
        /// <summary>
        /// To validate the input data
        /// </summary>
        /// <param name="value">user input data</param>
        /// <param name="validationContext">validationContext</param>
        /// <returns>Success or false with message</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] inValidString = InvalidInputConstants.InvalidData;
            string inputdata = (value ?? string.Empty).ToString();
            if (!string.IsNullOrEmpty(inputdata))
            {
                var inputDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputdata);
                Regex regex = new Regex(InvalidInputConstants.InvalidDataRegex);
                var invalidResults = inputDict.Where(data => regex.Match(data.Value.ToString()).Success || inValidString.Any(data.Value.ToString().ToLower().Contains)).ToList();
                return invalidResults.Any() ? new ValidationResult(FieldErrorMessageConstants.FieldInvalidData) : ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}