using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExtensionsLibrary
{
    public static class DataAnnotationsExtenstions
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>ValidationResult</returns>
        public static IEnumerable<ValidationResult> GetValidationErrors(this object obj)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);
            Validator.TryValidateObject(obj, context, validationResults, true);
            return validationResults;
        }

        /// <summary>
        /// Validates the specified object.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="results">The results.</param>
        /// <returns>Validation status true/false</returns>
        public static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
    }
}