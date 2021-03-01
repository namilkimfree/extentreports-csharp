﻿using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;
using AventStack.ExtentReports.Views.Html;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentHtmlReporter : BasicFileReporter
    {
        public new string ReporterName => "html";

        private static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private static readonly string HtmlTemplateFolderPath = Path.Combine(TemplateFolderPath, "Html");
        private ViewStyle _viewStyle = ViewStyle.SPA;

        private EncodedStringFactory _encodedStringFactory
        {
            set => RazorEngineManager.Instance.EncodedStringFactory = value;
        }

        public ExtentHtmlReporter(string folderPath) : base(folderPath)
        {
            Config = new ExtentHtmlReporterConfiguration(this);
            
            Initialize(Config);
        }

        public ExtentHtmlReporter(string folderPath, ViewStyle viewStyle) : this(folderPath)
        {
            _viewStyle = viewStyle;
        }

        public ExtentHtmlReporter(string folderPath, EncodedStringFactory encodedStringFactory) : this(folderPath)
        {
            _encodedStringFactory = encodedStringFactory;
        }

        public ExtentHtmlReporter(string folderPath, ViewStyle viewStyle, EncodedStringFactory encodedStringFactory) : this(folderPath)
        {
            _encodedStringFactory = encodedStringFactory;
            _viewStyle = viewStyle;
        }

        public ExtentHtmlReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);

            if (_viewStyle == ViewStyle.SPA)
            {
                var source = RazorEngineManager.Instance.Razor.RunCompile("SparkIndexSPA", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "index.html"), source);
            }
            else
            {
                var source = RazorEngineManager.Instance.Razor.RunCompile("Index", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "index.html"), source);
                source = RazorEngineManager.Instance.Razor.RunCompile("Dashboard", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "dashboard.html"), source);
                if (CategoryContext.Context.Count > 0)
                {
                    source = RazorEngineManager.Instance.Razor.RunCompile("Tag", typeof(ExtentHtmlReporter), this);
                    File.WriteAllText(Path.Combine(FolderSavePath, "tag.html"), source);
                }
                if (ExceptionInfoContext.Context.Count > 0)
                {
                    source = RazorEngineManager.Instance.Razor.RunCompile("Exception", typeof(ExtentHtmlReporter), this);
                    File.WriteAllText(Path.Combine(FolderSavePath, "exception.html"), source);
                }
            }
        }

        public override void Start()
        {
            base.Start();
            AddTemplates();
        }

        private void AddTemplates()
        {
            string[] templates = new string[]
            {
                "Dashboard",
                "Exception",
                "Index",
                "Tag",
                "Partials.Attributes",
                "Partials.AttributesView",
                "Partials.Head",
                "Partials.Log",
                "Partials.Navbar",
                "Partials.RecurseNodes",
                "Partials.Scripts",
                "Partials.Sidenav",
                "Partials.SparkBDD",
                "Partials.SparkStandard",
                "Partials.SparkStepDetails",
                "SparkIndexSPA",
                "Partials.SparkTestSPA",
                "Partials.SparkTagSPA",
                "Partials.SparkExceptionSPA",
                "Partials.SparkDashboardSPA"
            };

            TemplateLoadService.LoadTemplate<IHtmlMarker>(templates);
        }
    }
}
