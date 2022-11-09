using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Package.VZ
{
    public class VZ
    {
        #region 非托管类库
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll")]
        public static extern int VzLPRClient_Setup();
        /// <summary>
        /// 连接一台设备
        /// </summary>
        /// <param name="pStrIP"></param>
        /// <param name="wPort"></param>
        /// <param name="pStrUserName"></param>
        /// <param name="pStrPassword"></param>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll")]
        public static extern int VzLPRClient_Open(string pStrIP, ushort wPort, string pStrUserName, string pStrPassword);
        /// <summary>
        /// 播放画面
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll")]
        private static extern int VzLPRClient_StartRealPlay(int handle, IntPtr hWnd);
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="hRealHandle"></param>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll")]
        private static extern int VzLPRClient_StopRealPlay(int hRealHandle);
        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="pUserData"></param>
        /// <param name="pResult"></param>
        /// <param name="uNumPlates"></param>
        /// <param name="eResultType"></param>
        /// <param name="pImgFull"></param>
        /// <param name="pImgPlateClip"></param>
        /// <returns></returns>
        public delegate int VZLPRC_PLATE_INFO_CALLBACK(int handle, IntPtr pUserData, IntPtr pResult, uint uNumPlates, VZ_LPRC_RESULT_TYPE eResultType, IntPtr pImgFull, IntPtr pImgPlateClip);
        //识别结果的类型
        public enum VZ_LPRC_RESULT_TYPE : uint
        {
            VZ_LPRC_RESULT_REALTIME,		/*<实时识别结果*/
            VZ_LPRC_RESULT_STABLE,			/*<稳定识别结果*/
            VZ_LPRC_RESULT_FORCE_TRIGGER,	/*<调用“VzLPRClient_ForceTrigger”触发的识别结果*/
            VZ_LPRC_RESULT_IO_TRIGGER,		/*<外部IO信号触发的识别结果*/
            VZ_LPRC_RESULT_VLOOP_TRIGGER,	/*<虚拟线圈触发的识别结果*/
            VZ_LPRC_RESULT_MULTI_TRIGGER,	/*<由_FORCE_\_IO_\_VLOOP_中的一种或多种同时触发，具体需要根据每个识别结果的TH_PlateResult::uBitsTrigType来判断*/
            VZ_LPRC_RESULT_TYPE_NUM			/*<结果种类个数*/
        }
        /// <summary>
        /// 设置回调
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="func"></param>
        /// <param name="pUserData"></param>
        /// <param name="bEnableImage"></param>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int VzLPRClient_SetPlateInfoCallBack(int handle, VZLPRC_PLATE_INFO_CALLBACK func, IntPtr pUserData, int bEnableImage);
        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport(@"VZ\VzLPRSDK.dll")]
        private static extern int VzLPRClient_Close(int handle);

        /**
      *  @brief 将图像保存为JPEG到指定路径
      *  @param  [IN] pImgInfo 图像结构体，目前只支持默认的格式，即ImageFormatRGB
      *  @param  [IN] pFullPathName 设带绝对路径和JPG后缀名的文件名字符串
      *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
      *  @return 0表示成功，-1表示失败
      *  @note   给定的文件名中的路径需要存在
      *  @ingroup group_global
      */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_ImageSaveToJpeg(IntPtr pImgInfo, string pFullPathName, int nQuality);


        #region 结构体
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_MSG_PLATE_INFO
        {

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string plate;

            /// char[128]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
            public string img_path;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct TH_RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct VZ_TIMEVAL
        {
            public uint uTVSec;
            public uint uTVUSec;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct VzBDTime
        {
            public byte bdt_sec;    /*<秒，取值范围[0,59]*/
            public byte bdt_min;    /*<分，取值范围[0,59]*/
            public byte bdt_hour;   /*<时，取值范围[0,23]*/
            public byte bdt_mday;   /*<一个月中的日期，取值范围[1,31]*/
            public byte bdt_mon;    /*<月份，取值范围[1,12]*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] res1;     /*<预留*/
            public uint bdt_year;   /*<年份*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] res2;     /*<预留*/
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct TH_PlateResult
        {
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] license;      // 车牌号码

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] color;        // 车牌颜色

            public int nColor;			// 车牌颜色序号
            public int nType;			// 车牌类型
            public int nConfidence;	    // 车牌可信度
            public int nBright;		    // 亮度评价
            public int nDirection;		// 运动方向，0 unknown, 1 left, 2 right, 3 up , 4 down	
            public TH_RECT rcLocation;  //车牌位置
            public int nTime;           //识别所用时间
            public VZ_TIMEVAL tvPTS;    //识别时间点
            public uint uBitsTrigType;  //强制触发结果的类型，见TH_TRIGGER_TYPE_BIT
            public byte nCarBright;		//车的亮度
            public byte nCarColor;		//车的颜色

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] reserved0;    //为了对齐

            public uint uId;            //记录的编号
            public VzBDTime struBDTime; //分解时间

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 84, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] reserved;	    // 保留
        }
        #endregion
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(
                IntPtr hWnd,        // 信息发往的窗口的句柄
               int Msg,            // 消息ID
                int wParam,         // 参数1
                int lParam            // 参数2
            );
        private int huidaio(int handle, IntPtr pUserData, IntPtr pResult, uint uNumPlates, VZ_LPRC_RESULT_TYPE eResultType, IntPtr pImgFull, IntPtr pImgPlateClip)
        {
            if (eResultType != VZ_LPRC_RESULT_TYPE.VZ_LPRC_RESULT_REALTIME)
            {
                TH_PlateResult result = (TH_PlateResult)Marshal.PtrToStructure(pResult, typeof(TH_PlateResult));
                string strLicense = new string(result.license);

                VZ_LPR_MSG_PLATE_INFO plateInfo = new VZ_LPR_MSG_PLATE_INFO();
                plateInfo.plate = strLicense;

                // 屏蔽为保存图片到本地
                //string strFilePath = System.IO.Directory.GetCurrentDirectory() + "\\cap\\";
                //if (!Directory.Exists(strFilePath))
                //{
                //    Directory.CreateDirectory(strFilePath);
                //}
                //string path = strFilePath + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".jpg";
                //VzLPRClient_ImageSaveToJpeg(pImgFull, path, 100);
                //plateInfo.img_path = path;


                int size = Marshal.SizeOf(plateInfo);
                IntPtr intptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(plateInfo, intptr, true);

                PostMessage(this.handle, 0x901, (int)intptr, handle);
            }

            return 0;
        }
        #endregion
        string ip = "", port = "", user = "", pwd = "";
        public Int32 id = 0;
        IntPtr handle = IntPtr.Zero;
        VZLPRC_PLATE_INFO_CALLBACK ret = null;



        public VZ(string ip, string port, string user, string pwd)
        {
            VzLPRClient_Setup();
            this.ip = ip;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
        }
        public void Open()
        {
            id = VzLPRClient_Open(ip, (ushort)short.Parse(port), user, pwd);
        }
        public void Call(IntPtr handle)
        {
            this.handle = handle;
            this.ret = new VZLPRC_PLATE_INFO_CALLBACK(huidaio);
            VzLPRClient_SetPlateInfoCallBack(id, ret, IntPtr.Zero, 1);
        }
        public void Play(IntPtr webcam_Handle)
        {
            VzLPRClient_StartRealPlay(id, webcam_Handle);
        }
        [DllImport(@"VZ\VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetDeviceIP(int handle, ref byte ip, int max_count);
    }
}
