﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace JJ.Presentation.SaveText.AppService.Interface.Models
{
    [DataContract]
    public class Labels
    {
        [DataMember]
        public string Text { get; set; }
    }
}
