﻿using System.Collections.Generic;

namespace Vnr.Storage.API.Infrastructure.Models
{
    public class FormFileErrorModel
    {
        public bool IsError { get; set; }
        public IDictionary<string, string> Errors { get; set; }

        public FormFileErrorModel()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}