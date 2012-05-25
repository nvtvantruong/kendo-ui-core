namespace Kendo.Mvc.UI
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Infrastructure;
    using Kendo.Mvc.Resources;
    using Kendo.Mvc.UI.Html;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    public class DateTimePicker : DatePickerBase
    {
        static internal DateTime defaultMinDate = new DateTime(1899, 12, 31);
        static internal DateTime defaultMaxDate = new DateTime(2100, 1, 1);

        public DateTimePicker(ViewContext viewContext, IJavaScriptInitializer initializer, ViewDataDictionary viewData)
            : base(viewContext, initializer, viewData)
        {
            DateTimeFormatInfo dateTimeFormats = CultureInfo.CurrentCulture.DateTimeFormat;
            Format = dateTimeFormats.ShortDatePattern + " " + dateTimeFormats.ShortTimePattern;

            Min = defaultMinDate;
            Max = defaultMaxDate;

            MonthTemplate = new MonthTemplate();

            Dates = new List<DateTime>();

            Interval = 30;
        }

        public MonthTemplate MonthTemplate
        {
            get;
            private set;
        }

        public string Footer
        {
            get;
            set;
        }

        public string Start
        {
            get;
            set;
        }

        public string Depth
        {
            get;
            set;
        }

        public List<DateTime> Dates
        {
            get;
            set;
        }

        public int Interval
        {
            get;
            set;
        }

        public override void WriteInitializationScript(TextWriter writer)
        {
            var options = new Dictionary<string, object>();

            var animation = Animation.ToJson();

            if (animation.Keys.Any())
            {
                options["animation"] = animation["animation"];
            }

            if (ClientEvents.Keys.Any())
            {
                options["events"] = ClientEvents;
            }

            options["format"] = Format;
            
            if (ParseFormats.Any())
            {
                options["parseFormats"] = ParseFormats;
            }

            options["min"] = Min;
            options["max"] = Max;
            options["footer"] = Footer;

            options["depth"] = Depth;
            options["start"] = Start;

            var month = MonthTemplate.ToJson();

            if (month.Keys.Any())
            {
                options["month"] = month;
            }

            options["interval"] = Interval;
            
            if (Dates.Any())
            {
                options["dates"] = Dates;
            }
            
            writer.Write(Initializer.Initialize(Id, "DateTimePicker", options));

            base.WriteInitializationScript(writer);
        }

        protected override void WriteHtml(System.Web.UI.HtmlTextWriter writer)
        {
            Guard.IsNotNull(writer, "writer");

            DatePickerHtmlBuilderBase renderer = new DatePickerHtmlBuilderBase(this, "datetime");

            renderer.Build().WriteTo(writer);
            base.WriteHtml(writer);
        }

        public override void VerifySettings()
        {
            base.VerifySettings();

            if (Min > Max)
            {
                throw new ArgumentException(TextResource.MinPropertyMustBeLessThenMaxProperty.FormatWith("Min", "Max"));
            }
        }
    }
}
