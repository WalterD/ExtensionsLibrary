using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtensionsLibrary
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Gets the model state items.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="errorsOnly">if set to <c>true</c> [errors only].</param>
        /// <returns>List of ModelStateItem</returns>
        public static List<ModelStateItem> GetModelStateItems(this ModelStateDictionary modelState, bool errorsOnly)
        {
            var modelStateItems = new List<ModelStateItem>();
            if (modelState == null)
            {
                return modelStateItems;
            }

            foreach (var item in modelState)
            {
                ModelStateEntry itemValue = item.Value;
                ModelErrorCollection errors = item.Value.Errors;

                if (itemValue.ValidationState != ModelValidationState.Invalid && errorsOnly)
                {
                    continue;
                }

                var modelStateItem = new ModelStateItem
                {
                    ItemName = item.Key,
                    ModelValidationState = itemValue.ValidationState,
                    AttemptedValue = itemValue.AttemptedValue,
                };

                if (errors != null && errors.Count > 0)
                {
                    for (int i = 0; i < errors.Count; i++)
                    {
                        var errorMessage = string.IsNullOrEmpty(errors[i].ErrorMessage) ? errors[i].Exception.Message : errors[i].ErrorMessage;
                        modelStateItem.ErrorMessages.Add(errorMessage);
                    }
                }

                modelStateItems.Add(modelStateItem);
            }

            return modelStateItems;
        }

        /// <summary>
        /// ModelStateItem
        /// </summary>
        public class ModelStateItem
        {
            public ModelStateItem()
            {
                ErrorMessages = new List<string>();
            }

            public ModelValidationState ModelValidationState { get; set; }

            public string ItemName { get; set; }

            public string AttemptedValue { get; set; }

            public List<string> ErrorMessages { get; set; }

            public bool IsInvalid
            {
                get
                {
                    return ModelValidationState == ModelValidationState.Invalid;
                }
            }

            /// <summary>
            /// Errors the messages to string.
            /// </summary>
            /// <param name="errorSeparator">The error separator.</param>
            /// <returns>All errors in one string</returns>
            public string ErrorMessagesToString(string errorSeparator)
            {
                string message = string.Join(errorSeparator, ErrorMessages);
                return message ?? string.Empty;
            }
        }
    }
}