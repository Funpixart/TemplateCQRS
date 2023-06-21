using Microsoft.AspNetCore.Components;
using TemplateCQRS.WebApp.Components.Enums;

namespace TemplateCQRS.WebApp.Components.Elements
{
    public partial class InputDynamic<TEntity> : FunpixartDynamic<TEntity>
    {
        [Parameter] public InputTypes InputType { get; set; } = InputTypes.Default;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddAttributesBaseOnType();
        }

        private void AddAttributesBaseOnType()
        {
            if (InputType == InputTypes.Phone)
            {
                Attributes.Add("type", "tel");
                return;

            }
            var type = typeof(TEntity);

            if (type == typeof(string))
            {
                Attributes.Add("type", "text");
            }
            else if (type == typeof(sbyte) || type == typeof(bool))
            {
                Attributes.Add("type", "checkbox");
            }
            else if (type == typeof(DateOnly))
            {
                Attributes.Add("type", "date");
            }
            else if (type == typeof(DateTime))
            {
                Attributes.Add("type", "datetime");
            }
            else if (type == typeof(int))
            {
                Attributes.Add("type", "number");
            }
        }
    }
}
