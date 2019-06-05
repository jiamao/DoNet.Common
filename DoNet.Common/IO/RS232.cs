using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DoNet.Common.IO
{
    public static class RS232
    {
        #region "类头部"
#region "各种参数枚举"
    //极性位设置
    public Enum eDataParity
    {
        Parity_None = 0
        Parity_Odd
        Parity_Even
        Parity_Mark
    }
    //停止位设置
    public Enum eDataStopBit
        StopBit_1 = 0
        StopBit_2 = 2
    End Enum
    //数据位设置
    public Enum eDataBit
        Bit_5 = 5
        Bit_6 = 6
        Bit_7 = 7
        Bit_8 = 8
    End Enum
    //硬件握手设置
    public Enum eHwHandShake
        hhNone = 0
        hhRtsOn = 1
        hhRtsCts = 2
        hhDtrOn = 3
        hhDtrDsr = 4
    End Enum
    ////软件握手设置
    public Enum eSwHandShake
        shNone = 0
        shXonXoff = 1
    End Enum
    //缓冲区清除设置
    public Enum ePurgeBuffers
        PXAbort = &H2
        PXClear = &H8
        TxAbort = &H1
        txClear = &H4
    End Enum
    //串行通信硬件线路设置(EscapeFunction)
    public Enum eLines
        SetXoff = 1 //当接收到xoff字符时启动传输操作
        SetXon = 2 //当接收到XON字符时启动传输操作
        SetRts = 3 //将RTS线路升成高电位
        ClearRts = 4 //将RTS线路降成低电位
        SetDtr = 5 //将DTR线路升成高电位
        ClearDtr = 6 //将DTR线路降为低电位
        ResetDev = 7 //重置设备
        SetBreak = 8 //设置通信状态为中断（送出BREAK信号）
        ClearBreak = 9 //清除BREAK信号，使传输操作继续
    End Enum
    //调制解调器专用状态位设置
    <Flags()> public Enum eModemStatusBits
        ClearToSendOn = &H10
        DataSetReadOn = &H20
        RingIndicatorOn = &H40
        CarrierDetect = &H80
    End Enum
    //事件屏蔽设置
    <Flags()> public Enum eEventMasks
        RxChar = &H1 //输入缓冲区已收到一个字符
        RXFlag = &H2 //使用setcommstate函数设置的DCB结构中的等待字符已被传入输入缓冲区
        TxBufferEmpty = &H4 //在输出缓冲区中的数据已被完全送出
        ClearToSend = &H8 //CTS（Clear to send）线路发生变化
        DataSetReady = &H10 //DSR线路发生变化
        ReceiveLine = &H20 //CD线路信号发生变化
        Break = &H40 //收到BREAK信号
        StatusError = &H80 //线路状态错误，包括了CE——FRAME和CE——OVERRUN，CE——RXPARITY三种错误
        Ring = &H100 //检测到响铃信号
    End Enum
    //DCB设置常数
    <Flags()> public Enum eBitDef
        dcb_Binary = &H1
        dcb_parityCheck = &H2
        dcb_OutxCtsFlow = &H4
        dcb_OutxDsrFlow = &H8
        dcb_DtrControlMask = &H30
        dcb_DtrControlDisable = &H0
        dcb_DtrControlEnable = &H10
        dcb_DtrControlHandshake = &H20
        dcb_DsrSendsivity = &H40
        dcb_TXContinueOnXoff = &H80
        dcb_Outx = &H100
        dcb_InX = &H200
        dcb_ErrorChar = &H400
        dcb_NullStrip = &H800
        dcb_RtsControlMask = &H3000
        dcb_RtsControlToggle = &H3000
        dcb_RtsControldisable = &H0
        dcb_RtsControlEnable = &H1000
        dcb_RtscontrolHandShake = &H2000
        dcb_AbortOnError = &H4000
        dcb_Reserveds = &HFFFF8000
    End Enum
    //通信端口错误常数
    <Flags()> public Enum eCommError
        CE_DREAK = &H10
        CE_DNS = &H800
        CE_FRAME = &H8
        CE_IOE = &H400
        CE_MODE = &H8000
        CE_OOP = &H1000
        CE_PTO = &H200
        CE_OVERRUN = &H2
        CE_RXOVER = &H1
    End Enum
    //通信端口线路状态检测常数
    <Flags()> public Enum eCommErrorLine
        fCtsHold = &H1 //等待CTS信号准备传送
        fDsrHold = &H2 //等待DRS信号准备传送
        fRlsdHold = &H4 //等待RLAS信号准备传送
        fXoffHold = &H8 //收到XOFF字符停止传送
        fXoffSent = &H10 //已送出XOFF字符，停止传送
        fEof = &H20 //EOF字符已送出
        fTxim = &H40 //字符等待传送
    End Enum
    //通信速度的设置枚举
    public Enum eBaudrates
        BR_110 = 100
        BR_300 = 300
        BR_600 = 600
        BR_1200 = 1200
        BR_2400 = 2400
        BR_4800 = 4800
        BR_9600 = 9600
        BR_14400 = 14400
        BR_19200 = 19200
        BR_38400 = 38400
        BR_56000 = 56000
        BR_57600 = 57600
        BR_115200 = 115200
    End Enum
#endregion
#region "结构"
    //Device Control block结构声明
    <StructLayout(LayoutKind.Sequential, Pack:=1)> public Structure DCB
        public DCBlength As Int32
        public BaudRate As Int32
        public Bits1 As Int32
        public wReserved As Int16
        public XonLim As Int16
        public XoffLim As Int16
        public ByteSize As Byte
        public Parity As Byte
        public StopBits As Byte
        public XonChar As Char
        public XoffChar As Char
        public ErrorChar As Char
        public EofChar As Char
        public EvtChar As Char
        public wReserved2 As Int16
    End Structure
    //通信端口状态结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> public Structure COMSTAT
        Dim fBitFields As Int32
        Dim cbInQue As Int32
        Dim cbOutQue As Int32
    End Structure
    //超时设置结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> public Structure COMMTIMEOUTS
        public ReadIntervalTimeout As Int32
        public ReadTotalTimeoutMultiplier As Int32
        public ReadTotalTimeoutconstant As Int32
        public WriteTotalTimeoutMultiplier As Int32
        public WriteTotalTimeoutconstant As Int32
    End Structure
    //通信端口配置结构
    <StructLayout(LayoutKind.Sequential, Pack:=8)> public Structure COMMCONFIG
        public dwSize As Int32
        public wVersion As Int16
        public wReserved As Int16
        public dcbx As DCB
        public dwProviderSubType As Int32
        public dwProviderOffset As Int32
        public dwProviderSize As Int32
        public wcProviderData As Int16
    End Structure
    //异步传输的设置结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> public Structure OVERLAPPED
        public Internal As Int32
        public InternalHigh As Int32
        public Offset As Int32
        public OffsetHigh As Int32
        public hEvent As IntPtr
    End Structure
#endregion
#region "常数"
    private const PURGE_RXABORT As Integer = &H2
    private const PURGE_RXCLEAR As Integer = &H8
    private const PURGE_TXABORT As Integer = &H1
    private const PURGE_TXCLEAR As Integer = &H4
    private const GENERIC_READ As Integer = &H80000000 //打开通信端口时的参数
    private const GENERIC_WRITE As Integer = &H40000000 //打开通信端口时的参数
    private const OPEN_EXISTING As Integer = 3 //打开通信端口时的参数
    private const INVALID_HANDLE As Integer = -1 //通信端口关闭时的Handle值
    private const IO_BUFFER_SIZE As Integer = 1024
    private const FILE_FLAG_OVERLAPPED As Int32 = &H40000000
    private const ERROR_IO_PENDING As Int32 = 997
    private const WAIT_OBJECT_0 As Int32 = 0
    private const ERROR_IO_INCOMPLETE As Int32 = 996
    private const WAIT_TIMEOUT As Int32 = &H102&
    private const INFINITE As Int32 = &HFFFFFFFF
    private const TIMER_INTERVAL As Int16 = 10 //定时器所使用的时间隔
#endregion
#region "局部变量"
    private mabtRxBuf As Byte()
    //private meMode As eMode
    private moThreadTx As Thread
    private moThreadRx As Thread
    private moEvents As Thread
    private miTmpBytes2Read As Int32
    private meMask As eEventMasks
    private mbDisposed As Boolean
    private mbUseXonXoff As Boolean
    private mbEnableEvents As Boolean
    private miBufThreshold As Int32 = 1
    private muOvlE As OVERLAPPED
    private muOvlW As OVERLAPPED
    private muOvlR As OVERLAPPED
    private mhRS As IntPtr = INVALID_HANDLE    //// 串口的handle								
    private miPort As Integer = 1   ////  默认通信端口为COM1
    private mfPortOpen As Boolean //通信端口打开状态
    private meBaudrate As eBaudrates = eBaudrates.BR_9600 //9600bps
    private miTimeout As Int32 = 70   //// 
    private miBaudRate As Int32 = 9600
    private meParity As eDataParity = eDataParity.Parity_None //无同位校验
    private mstopBit As eDataStopBit = eDataStopBit.StopBit_1 //停止校验位为1
    private meDataBit As eDataBit = eDataBit.Bit_8 //数据长度为8
    private meHwHandshake As eHwHandShake = eHwHandShake.hhNone //默认为无硬件握手
    private meSwHandshake As eSwHandShake = eSwHandShake.shNone //默认为无软件握手
    private meCommEvent As eEventMasks //通信事件号码
    private meCommError As eCommError //错误号码
    private mDCBBit As eBitDef //控制区块变量
    private mReadLen As Integer //每次READ命令的读取字节数
    private mRThreshold As Integer //触发ondatareceived事件阈值
    private mCDHolding As Boolean //CD的针脚状态
    private mDSRHolding As Boolean //DSR的针脚状态
    private mCTSHolding As Boolean //CTS的针脚状态
    private mRIHolding As Boolean //RI的针脚状态
    private meStopBit As eDataStopBit = 0
    private miDataBit As Int32 = 8
    private mHE As GCHandle
    private mHR As GCHandle
    private mHW As GCHandle
    private mDTR As Boolean //DTR状态
    private mRTS As Boolean //RTS状态
    private mfTimer As Boolean //记录定时器的状态
    private miBufferSize As Integer = 512 //默认缓冲区512bytes
#endregion

        /// <summary>
        /// 串口通信系统api
        /// </summary>
        public static class RS232API
        { 
        
        }
    }
}
