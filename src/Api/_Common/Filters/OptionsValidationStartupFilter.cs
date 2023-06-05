namespace Api.Common.Filters;

/// <summary>Implemented as described on https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-asp-net-core/ to help with Options Validation</summary>
public class OptionsValidationStartupFilter : IStartupFilter
{
    private readonly IEnumerable<IValidateSettingsService> _validatableObjects;

    #region Constructor
    public OptionsValidationStartupFilter(IEnumerable<IValidateSettingsService> validationObjects)
    {
        _validatableObjects = validationObjects;
    }
    #endregion

    #region IStartupFilter Implementation
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        foreach (var validatableObject in _validatableObjects)
        {
            validatableObject.Validate();
        }

        //don't alter the configuration
        return next;
    }
    #endregion
}
