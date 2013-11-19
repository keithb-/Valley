﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Valley.Models;

namespace Valley.Formatting
{
    //http://lonetechie.com/2012/09/23/web-api-generic-mediatypeformatter-for-file-upload/
    public class GenericMediaTypeFormatter : MediaTypeFormatter
    {
        public GenericMediaTypeFormatter()
        {
            //TODO: Load this list of supported file types from the configuration.
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(GenericContent);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {

            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var Parts = await content.ReadAsMultipartAsync();
            var FileContent = Parts.Contents.First(x =>
                SupportedMediaTypes.Contains(x.Headers.ContentType));

            var DataString = "";
            foreach (var Part in Parts.Contents.Where(x => x.Headers.ContentDisposition.DispositionType == "form-data"
                                                        && x.Headers.ContentDisposition.Name == "\"data\""))
            {
                var Data = await Part.ReadAsStringAsync();
                DataString = Data;
            }

            string FileName = FileContent.Headers.ContentDisposition.FileName;
            string MediaType = FileContent.Headers.ContentType.MediaType;

            using (var Imgstream = await FileContent.ReadAsStreamAsync())
            {
                return new GenericContent { ContentType = MediaType, Body = ReadFully(Imgstream) };
            }
        }

        private byte[] ReadFully(Stream input)
        {
            var Buffer = new byte[16 * 1024];
            using (var Ms = new MemoryStream())
            {
                int Read;
                while ((Read = input.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    Ms.Write(Buffer, 0, Read);
                }
                return Ms.ToArray();
            }
        }
    }
}