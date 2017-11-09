using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace HiveTools
{
    [Serializable]
    public class ConfigColor // This class will store and manipulate color values in human readable form
    {

        public const string fileName = "ColorConfig.xml";

        // Default data set colors
        [XmlIgnore]
        public Color ColorRepeat { get; set; } = Color.Orange;
        [XmlElement("ColorRepeat")]
        public string ColorRepeatAsStr
        {
            get { return "#" +ColorRepeat.ToArgb().ToString("X"); }
            set { ColorRepeat = System.Drawing.ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color ColorDuplicate { get; set; } = Color.Olive;
        [XmlElement("ColorDuplicate")]
        public string ColorDuplicateAsStr
        {
            get { return "#" + ColorDuplicate.ToArgb().ToString("X"); }
            set { ColorDuplicate = System.Drawing.ColorTranslator.FromHtml(value); }
        }


        [XmlIgnore]
        public Color ColorMissing { get; set; } = Color.OrangeRed;
        [XmlElement("ColorMissing")]
        public string ColorMissingAsStr
        {
            get { return "#" + ColorMissing.ToArgb().ToString("X"); }
            set { ColorMissing = System.Drawing.ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color ColorNull { get; set; } = Color.Orchid;
        [XmlElement("ColorNull")]
        public string ColorNullAsStr
        {
            get { return "#" + ColorNull.ToArgb().ToString("X"); }
            set { ColorNull = System.Drawing.ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color ColorZero { get; set; } = Color.CornflowerBlue;
        [XmlElement("ColorZero")]
        public string ColorZeroAsStr
        {
            get { return "#" + ColorZero.ToArgb().ToString("X"); }
            set { ColorZero = System.Drawing.ColorTranslator.FromHtml(value); }
        }


        [XmlIgnore]
        public Color ColorInvalid { get; set; } = Color.Red;
        [XmlElement("ColorInvalid")]
        public string ColorInvalidAsStr
        {
            get { return "#" + ColorInvalid.ToArgb().ToString("X"); }
            set { ColorInvalid = System.Drawing.ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color ColorValid { get; set; } = Color.Green;
        [XmlElement("ColorValid")]
        public string ColorValidAsStr
        {
            get { return "#" + ColorValid.ToArgb().ToString("X"); }
            set { ColorValid = System.Drawing.ColorTranslator.FromHtml(value); }
        }
        
    }

    class ColorPalete // This is color helper class, will be used like interface betweeen ConfigColor and ConfigClass
    {
        public Color colorRepeat = Color.Orange;
        public Color colorDuplicate = Color.Olive;
        public Color colorMissing = Color.OrangeRed;
        public Color colorNull = Color.Orchid;
        public Color colorZero = Color.CornflowerBlue;
        public Color colorInvalid = Color.Red;
        public Color colorValid = Color.Green;
    }

}
 


