using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.APP.Utilities
{
    public static class Global
    {
        public static string? DeviceId { get; set; }
        public static string? SmoApiUsername { get; set; }
        public static string? SmoApiPassword { get; set; }
        public static string? SmoApiUrl { get; set; }
        public static string? PathSaveFile { get; set; }
        public static string? Connection { get; set; }
        public static string? DetectApiUrl { get; set; }
        public static string? DetectFilePath { get; set; }
        public static string? TimeService { get; set; }
        public static List<TblMdCamera> lstCamera { get; set; } = new List<TblMdCamera>();
        public static LibVLC _libVLC { get; set; }
    }
}
