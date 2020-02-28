using System;
using System.IO;

using Neurotec.Biometrics;
using Neurotec.Images;
using Neurotec.Licensing;

namespace FDEnroll
{
    class VerilookToolException : ApplicationException
    {
        public VerilookToolException(string msg) : base(msg) { }
    }

    class VerilookUtils : IDisposable
    {
        private const string components = "Biometrics.FaceExtraction";
        private const string errormsg = "Verilook Engine: ";
        private NLExtractor extractor = null;

        public VerilookUtils()
        {
        }

        public void Initialize()
        {
            bool b = true;
            try
            {
                // check license
                if (!NLicense.ObtainComponents("/local", 5000, components))
                    b = false;

                // Create an extractor
                extractor = new NLExtractor();

                // Set extractor template size (recommended, for enroll to database, is large) (optional)
                extractor.TemplateSize = NleTemplateSize.Large;
            }
            catch (Exception e)
            {
                string msg = e.InnerException != null ? e.InnerException.Message : e.Message;
                throw new VerilookToolException(errormsg + msg);
            }
            if (!b)
                throw new VerilookToolException("Obtaining Verilook License failed");
        }

        #region IDisposable Members

        public void Dispose()
        {
            NLicense.ReleaseComponents(components);

            if (extractor != null)
                extractor.Dispose();
        }

        #endregion

        public byte[] ExtractTemplate(DataSetMain.ImageDataTable table)
        {
            if (extractor == null)
                return new byte[0];

            NLTemplate template = null;

            try
            {
                // Start extraction of face template from a stream (sequence of images)
                extractor.ExtractStart(table.Rows.Count);

                NleExtractionStatus extractionStatus = NleExtractionStatus.None;
                // process stream of images
                foreach (DataSetMain.ImageRow row in table)
                {
                    // read image
                    NImage image = NImage.FromStream(new MemoryStream(row.Image));
                    //????????????????????? Changing  to NPixelFormat.Grayscale
                    using (NGrayscaleImage grayscaleImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, image))
                    {
                        if (image != null)
                            image.Dispose();
                        // use image as another frame of stream
                        NleDetectionDetails details;
                        int baseFrameIndex;
                        extractionStatus = extractor.ExtractNext(grayscaleImage, out details, out baseFrameIndex, out template);
                    }
                }
                if (extractionStatus == NleExtractionStatus.TemplateCreated)
                {
                    // return compressed template
                    return template.Save().ToByteArray();
                }
            }
            catch (Exception e)
            {
                string msg = e.InnerException != null ? e.InnerException.Message : e.Message;
                throw new VerilookToolException(errormsg + msg);
            }
            finally
            {
                if (template != null)
                    template.Dispose();
            }

            return new byte[0];
        }

    }
}
