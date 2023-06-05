using Application.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class AppSettings : IValidateSettingsService
    {
        /// <summary>Sample Url Setting</summary>
        [Display(Name = "AppSettings.WebUrlSample")]
        [Required, Url]
        public string WebUrlSample { get; set; }

        /// <summary>Sample Integer Settings</summary>
        [Display(Name = "AppSettings.CacheingInMinutesSample")]
        [Required]
        [Range(1, 1440)]
        public int CacheingInMinutesSample { get; set; }

        /// <summary>Threshold In Milliseconds. Used by PerformanceBehavior for logging long-running queries and commands</summary>
        [Display(Name = "AppSettings.PerformanceThresholdInMilliseconds")]
        [Required]
        [Range(1000, 10000)]
        public int PerformanceThresholdInMilliseconds { get; set; }

        #region IValidateSettingsService Implementation
        public void Validate()
        {
            // Perform DataAnnotation Validation
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
        #endregion
    }
}