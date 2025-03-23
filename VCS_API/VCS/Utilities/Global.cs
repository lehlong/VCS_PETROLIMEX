using LibVLCSharp.Shared;
using Microsoft.ML.OnnxRuntime;
using VCS.DbContext.Entities.MD;

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
        public static string? VcsUrl { get; set; }
        public static uint CropWidth { get; set; }
        public static uint CropHeight { get; set; }
        public static List<TblMdCamera> lstCamera { get; set; } = new List<TblMdCamera>();
        public static LibVLC _libVLC { get; set; }
        public static InferenceSession _session;

        public static Button OpenCheckIn = new Button();
        public static Button OpenCheckOut = new Button();

        public static dynamic np { get; set; }
        public static dynamic cv2 { get; set; }
        public static dynamic ocr_module { get; set; }
    }
}
