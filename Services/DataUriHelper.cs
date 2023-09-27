
using System;
namespace SagErpBlazor.Services
{

    public static class DataUriHelper
    {
        public static bool IsImageDataUri(string dataUri)
        {
            // Ensure the input is a Data URI scheme
            if (!dataUri.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Extract the MIME type part from the Data URI
            int mimeTypeStart = "data:".Length;
            int mimeTypeEnd = dataUri.IndexOf(';', mimeTypeStart);
            if (mimeTypeEnd == -1)
            {
                mimeTypeEnd = dataUri.IndexOf(',', mimeTypeStart);
            }

            if (mimeTypeEnd == -1)
            {
                return false; // Invalid Data URI
            }

            string mimeType = dataUri.Substring(mimeTypeStart, mimeTypeEnd - mimeTypeStart);

            // Check if the MIME type indicates an image
            return IsImageMimeType(mimeType);
        }

        public static string GetMimeType(string dataUri)
        {
            string mimeType = "";
            if (!dataUri.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return mimeType;
            }

            // Extract the MIME type part from the Data URI
            int mimeTypeStart = "data:".Length;
            int mimeTypeEnd = dataUri.IndexOf(';', mimeTypeStart);
            if (mimeTypeEnd == -1)
            {
                mimeTypeEnd = dataUri.IndexOf(',', mimeTypeStart);
            }

            if (mimeTypeEnd == -1)
            {
                return mimeType; // Invalid Data URI
            }

             mimeType = dataUri.Substring(mimeTypeStart, mimeTypeEnd - mimeTypeStart);

            return GetImageTypeFromMimeTypeString(mimeType);
        }

        public static bool IsPdfDataUri(string dataUri)
        {
            // Ensure the input is a Data URI scheme
            if (!dataUri.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Extract the MIME type part from the Data URI
            int mimeTypeStart = "data:".Length;
            int mimeTypeEnd = dataUri.IndexOf(';', mimeTypeStart);
            if (mimeTypeEnd == -1)
            {
                mimeTypeEnd = dataUri.IndexOf(',', mimeTypeStart);
            }

            if (mimeTypeEnd == -1)
            {
                return false; // Invalid Data URI
            }

            string mimeType = dataUri.Substring(mimeTypeStart, mimeTypeEnd - mimeTypeStart);

            // Check if the MIME type indicates a PDF
            return IsPdfMimeType(mimeType);
        }

        private static bool IsImageMimeType(string mimeType)
        {
            // Add more image MIME types if needed
            string[] imageMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            return Array.Exists(imageMimeTypes, mime => mime.Equals(mimeType, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsPdfMimeType(string mimeType)
        {
            return mimeType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase);
        }
        private static string GetImageTypeFromMimeTypeString(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpeg":
                    return ".jpeg";
                case "image/png":
                    return ".png";
                case "image/gif":
                    return ".gif";
                case "image/bmp":
                    return ".bmp";
                default:
                    return null; // Unknown image type
            }
        }
    }

}
