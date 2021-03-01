using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

using System;

namespace AventStack.ExtentReports.Reporter.TemplateEngine
{
    internal sealed class RazorEngineManager
    {
        private bool _initialized = false;
        private EncodedStringFactory _encodedStringFactory = EncodedStringFactory.RawStringFactory;
        public IRazorEngineService Razor => Engine.Razor;

        internal EncodedStringFactory EncodedStringFactory
        {
            set
            {
                _encodedStringFactory = value;
                ChangeEncodedStringFactory(_encodedStringFactory);
            }
        }

        private void ChangeEncodedStringFactory(EncodedStringFactory encodedStringFactory)
        {
            _initialized = false;
            _encodedStringFactory = encodedStringFactory;
            InitializeRazor();
        }

        internal void InitializeRazor()
        {
            if (_initialized)
                return;

            TemplateServiceConfiguration templateConfig = new TemplateServiceConfiguration()
            {
                DisableTempFileLocking = true,
                EncodedStringFactory = GetEncodedStringFactory(),
                CachingProvider = new DefaultCachingProvider(x => { })
            };

            var service = RazorEngineService.Create(templateConfig);
            Engine.Razor = service;

            _initialized = true;

            IEncodedStringFactory GetEncodedStringFactory()
            {
                IEncodedStringFactory encodedStringFactory;

                switch (_encodedStringFactory)
                {
                    case EncodedStringFactory.RawStringFactory:
                        encodedStringFactory = new RawStringFactory();
                        break;
                    case EncodedStringFactory.HtmlEncodedStringFactory:
                        encodedStringFactory = new HtmlEncodedStringFactory();
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                return encodedStringFactory;
            }
        } 

        private static readonly Lazy<RazorEngineManager> lazy =
        new Lazy<RazorEngineManager>(() => new RazorEngineManager());

        public static RazorEngineManager Instance { get { return lazy.Value; } }

        private RazorEngineManager()
        {
            InitializeRazor();
        }
    }
}
