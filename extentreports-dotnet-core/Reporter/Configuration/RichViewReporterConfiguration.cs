using System;

namespace AventStack.ExtentReports.Reporter.Configuration
{
    public class RichViewReporterConfiguration : BasicFileConfiguration
    {
        public RichViewReporterConfiguration(AbstractReporter reporter) : base(reporter)
        { }

        public Theme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                UserConfigurationMap["theme"] = Enum.GetName(typeof(Theme), _theme).ToLower();
            }
        }

        public DashboardStyle DashboardStyle
        {
            get
            {
                return _dashboardStyle;
            }
            set
            {
                _dashboardStyle = value;
                if (_dashboardStyle == DashboardStyle.AccordionChart)
                    CSS = _dashboardStyleDefaultCssValue;
                UserConfigurationMap["dashboardStyle"] = Enum.GetName(typeof(DashboardStyle), _dashboardStyle).ToLower();
            }
        }
        
        public string CSS
        {
            get
            {
                return _css;
            }
            set
            {
                _css = value;
                UserConfigurationMap["css"] = _css;
            }
        }

        public string JS
        {
            get
            {
                return _js;
            }
            set
            {
                _js = value;
                UserConfigurationMap["js"] = _js;
            }
        }

        public Boolean EnableTimeline
        {
            get
            {
                return _enableTimeline;
            }
            set
            {
                _enableTimeline = value;
                UserConfigurationMap["enableTimeline"] = _enableTimeline.ToString().ToLower();
            }
        }

        private Theme _theme;
        private DashboardStyle _dashboardStyle;
        private string _css;
        private string _js;
        private bool _enableTimeline;
        private string _dashboardStyleDefaultCssValue =
            @"

                .card-header a:before {
                    float: right !important;
                    font-family: FontAwesome;
                    content:""\f068"";
                    padding-right: 5px;
                }
                .card-header a.collapsed:before {
                    float: right !important;
                    content:""\f067"";
                }
                .card-header a:hover, 
                .card-header a:active, 
                .card-header a:focus  {
                    text-decoration:none;
                }
            ";
    }
}
