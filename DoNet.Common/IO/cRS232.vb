Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class cRS232
    Public WithEvents Timer1 As System.Windows.Forms.Timer    '声明一个定时器
#Region "类头部"
#Region "各种参数枚举"
    '极性位设置
    Public Enum eDataParity
        Parity_None = 0
        Parity_Odd
        Parity_Even
        Parity_Mark
    End Enum
    '停止位设置
    Public Enum eDataStopBit
        StopBit_1 = 0
        StopBit_2 = 2
    End Enum
    '数据位设置
    Public Enum eDataBit
        Bit_5 = 5
        Bit_6 = 6
        Bit_7 = 7
        Bit_8 = 8
    End Enum
    '硬件握手设置
    Public Enum eHwHandShake
        hhNone = 0
        hhRtsOn = 1
        hhRtsCts = 2
        hhDtrOn = 3
        hhDtrDsr = 4
    End Enum
    ''软件握手设置
    Public Enum eSwHandShake
        shNone = 0
        shXonXoff = 1
    End Enum
    '缓冲区清除设置
    Public Enum ePurgeBuffers
        PXAbort = &H2
        PXClear = &H8
        TxAbort = &H1
        txClear = &H4
    End Enum
    '串行通信硬件线路设置(EscapeFunction)
    Public Enum eLines
        SetXoff = 1 '当接收到xoff字符时启动传输操作
        SetXon = 2 '当接收到XON字符时启动传输操作
        SetRts = 3 '将RTS线路升成高电位
        ClearRts = 4 '将RTS线路降成低电位
        SetDtr = 5 '将DTR线路升成高电位
        ClearDtr = 6 '将DTR线路降为低电位
        ResetDev = 7 '重置设备
        SetBreak = 8 '设置通信状态为中断（送出BREAK信号）
        ClearBreak = 9 '清除BREAK信号，使传输操作继续
    End Enum
    '调制解调器专用状态位设置
    <Flags()> Public Enum eModemStatusBits
        ClearToSendOn = &H10
        DataSetReadOn = &H20
        RingIndicatorOn = &H40
        CarrierDetect = &H80
    End Enum
    '事件屏蔽设置
    <Flags()> Public Enum eEventMasks
        RxChar = &H1 '输入缓冲区已收到一个字符
        RXFlag = &H2 '使用setcommstate函数设置的DCB结构中的等待字符已被传入输入缓冲区
        TxBufferEmpty = &H4 '在输出缓冲区中的数据已被完全送出
        ClearToSend = &H8 'CTS（Clear to send）线路发生变化
        DataSetReady = &H10 'DSR线路发生变化
        ReceiveLine = &H20 'CD线路信号发生变化
        Break = &H40 '收到BREAK信号
        StatusError = &H80 '线路状态错误，包括了CE——FRAME和CE——OVERRUN，CE——RXPARITY三种错误
        Ring = &H100 '检测到响铃信号
    End Enum
    'DCB设置常数
    <Flags()> Public Enum eBitDef
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
    '通信端口错误常数
    <Flags()> Public Enum eCommError
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
    '通信端口线路状态检测常数
    <Flags()> Public Enum eCommErrorLine
        fCtsHold = &H1 '等待CTS信号准备传送
        fDsrHold = &H2 '等待DRS信号准备传送
        fRlsdHold = &H4 '等待RLAS信号准备传送
        fXoffHold = &H8 '收到XOFF字符停止传送
        fXoffSent = &H10 '已送出XOFF字符，停止传送
        fEof = &H20 'EOF字符已送出
        fTxim = &H40 '字符等待传送
    End Enum
    '通信速度的设置枚举
    Public Enum eBaudrates
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
#End Region
#Region "结构"
    'Device Control block结构声明
    <StructLayout(LayoutKind.Sequential, Pack:=1)> Public Structure DCB
        Public DCBlength As Int32
        Public BaudRate As Int32
        Public Bits1 As Int32
        Public wReserved As Int16
        Public XonLim As Int16
        Public XoffLim As Int16
        Public ByteSize As Byte
        Public Parity As Byte
        Public StopBits As Byte
        Public XonChar As Char
        Public XoffChar As Char
        Public ErrorChar As Char
        Public EofChar As Char
        Public EvtChar As Char
        Public wReserved2 As Int16
    End Structure
    '通信端口状态结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> Public Structure COMSTAT
        Dim fBitFields As Int32
        Dim cbInQue As Int32
        Dim cbOutQue As Int32
    End Structure
    '超时设置结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> Public Structure COMMTIMEOUTS
        Public ReadIntervalTimeout As Int32
        Public ReadTotalTimeoutMultiplier As Int32
        Public ReadTotalTimeoutConstant As Int32
        Public WriteTotalTimeoutMultiplier As Int32
        Public WriteTotalTimeoutConstant As Int32
    End Structure
    '通信端口配置结构
    <StructLayout(LayoutKind.Sequential, Pack:=8)> Public Structure COMMCONFIG
        Public dwSize As Int32
        Public wVersion As Int16
        Public wReserved As Int16
        Public dcbx As DCB
        Public dwProviderSubType As Int32
        Public dwProviderOffset As Int32
        Public dwProviderSize As Int32
        Public wcProviderData As Int16
    End Structure
    '异步传输的设置结构
    <StructLayout(LayoutKind.Sequential, Pack:=1)> Public Structure OVERLAPPED
        Public Internal As Int32
        Public InternalHigh As Int32
        Public Offset As Int32
        Public OffsetHigh As Int32
        Public hEvent As IntPtr
    End Structure
#End Region
#Region "常数"
    Private Const PURGE_RXABORT As Integer = &H2
    Private Const PURGE_RXCLEAR As Integer = &H8
    Private Const PURGE_TXABORT As Integer = &H1
    Private Const PURGE_TXCLEAR As Integer = &H4
    Private Const GENERIC_READ As Integer = &H80000000 '打开通信端口时的参数
    Private Const GENERIC_WRITE As Integer = &H40000000 '打开通信端口时的参数
    Private Const OPEN_EXISTING As Integer = 3 '打开通信端口时的参数
    Private Const INVALID_HANDLE As Integer = -1 '通信端口关闭时的Handle值
    Private Const IO_BUFFER_SIZE As Integer = 1024
    Private Const FILE_FLAG_OVERLAPPED As Int32 = &H40000000
    Private Const ERROR_IO_PENDING As Int32 = 997
    Private Const WAIT_OBJECT_0 As Int32 = 0
    Private Const ERROR_IO_INCOMPLETE As Int32 = 996
    Private Const WAIT_TIMEOUT As Int32 = &H102&
    Private Const INFINITE As Int32 = &HFFFFFFFF
    Private Const TIMER_INTERVAL As Int16 = 10 '定时器所使用的时间隔
#End Region
#Region "局部变量"
    Private mabtRxBuf As Byte()
    'Private meMode As eMode
    Private moThreadTx As Thread
    Private moThreadRx As Thread
    Private moEvents As Thread
    Private miTmpBytes2Read As Int32
    Private meMask As eEventMasks
    Private mbDisposed As Boolean
    Private mbUseXonXoff As Boolean
    Private mbEnableEvents As Boolean
    Private miBufThreshold As Int32 = 1
    Private muOvlE As OVERLAPPED
    Private muOvlW As OVERLAPPED
    Private muOvlR As OVERLAPPED
    Private mhRS As IntPtr = INVALID_HANDLE    '// 串口的handle								
    Private miPort As Integer = 1   '//  默认通信端口为COM1
    Private mfPortOpen As Boolean '通信端口打开状态
    Private meBaudrate As eBaudrates = eBaudrates.BR_9600 '9600bps
    Private miTimeout As Int32 = 70   '// 
    Private miBaudRate As Int32 = 9600
    Private meParity As eDataParity = eDataParity.Parity_None '无同位校验
    Private mstopBit As eDataStopBit = eDataStopBit.StopBit_1 '停止校验位为1
    Private meDataBit As eDataBit = eDataBit.Bit_8 '数据长度为8
    Private meHwHandshake As eHwHandShake = eHwHandShake.hhNone '默认为无硬件握手
    Private meSwHandshake As eSwHandShake = eSwHandShake.shNone '默认为无软件握手
    Private meCommEvent As eEventMasks '通信事件号码
    Private meCommError As eCommError '错误号码
    Private mDCBBit As eBitDef '控制区块变量
    Private mReadLen As Integer '每次READ命令的读取字节数
    Private mRThreshold As Integer '触发ondatareceived事件阈值
    Private mCDHolding As Boolean 'CD的针脚状态
    Private mDSRHolding As Boolean 'DSR的针脚状态
    Private mCTSHolding As Boolean 'CTS的针脚状态
    Private mRIHolding As Boolean 'RI的针脚状态
    Private meStopBit As eDataStopBit = 0
    Private miDataBit As Int32 = 8
    Private mHE As GCHandle
    Private mHR As GCHandle
    Private mHW As GCHandle
    Private mDTR As Boolean 'DTR状态
    Private mRTS As Boolean 'RTS状态
    Private mfTimer As Boolean '记录定时器的状态
    Private miBufferSize As Integer = 512 '默认缓冲区512bytes
#End Region
#Region "属性"
    '通信端口号码属性设置
    Public Property CommPort() As Integer
        Get
            Return miPort
        End Get
        Set(ByVal value As Integer)
            miPort = value
        End Set
    End Property
    '''设置端口开关
    ''' 设置会调用OpenCom及CloseCom
    Public Property PortOpen() As Boolean
        Get
            Return mfPortOpen
        End Get
        Set(ByVal value As Boolean)
            If value Then '打开
                If mfPortOpen Then '已经打开
                    Throw New Exception("通信端口已打开")
                    Exit Property
                End If
                mfPortOpen = True
                If Not OpenCOM() Then
                    Throw New Exception("通信端口打开错误")
                    Exit Property
                End If
                mfTimer = True
                If Not Timer1.Enabled Then
                    Timer1.Start()
                End If

            Else '关闭通信端口
                mfPortOpen = False
                If Not CloseCOM() Then '调用关闭端口
                    Throw New Exception("通信端口关闭错误")
                    Exit Property
                End If
            End If
        End Set
    End Property
    '设置通信速度，利用枚举比较方便
    Public Property BaudRate() As eBaudrates
        Get
            Return meBaudrate
        End Get
        Set(ByVal value As eBaudrates)
            meBaudrate = value
        End Set
    End Property
    '************************************************************
    '硬件握手设置
    '************************************************************
    Public Property hwhandShaking() As eHwHandShake
        Get
            Return meHwHandshake
        End Get
        Set(ByVal value As eHwHandShake)
            meHwHandshake = value
        End Set
    End Property
    '************************************************************
    '软件握手设置
    '************************************************************
    Public Property swhandShaking() As eSwHandShake
        Get
            Return meSwHandshake
        End Get
        Set(ByVal value As eSwHandShake)
            meSwHandshake = value
        End Set
    End Property
    '数据位设置
    Public Property DataBit() As eDataBit
        Get
            Return meDataBit
        End Get
        Set(ByVal Value As eDataBit)
            meDataBit = Value
        End Set
    End Property
    '停止位设置
    Public Property StopBit() As eDataStopBit
        Get
            Return meStopBit
        End Get
        Set(ByVal Value As eDataStopBit)
            meStopBit = Value
        End Set
    End Property
    '同位校验设置
    Public Property Parity() As eDataParity
        Get
            Return meParity
        End Get
        Set(ByVal Value As eDataParity)
            meParity = Value
        End Set
    End Property
    '通信事件（只读）
    Public ReadOnly Property CommEvent() As eEventMasks
        Get
            Return meCommEvent
        End Get
    End Property
    '通信错误（只读）
    Public ReadOnly Property CommError() As eCommError
        Get
            Return meCommError
        End Get
    End Property
    '每次读取操作的字节数，使用read命令时可以指定
    Public Property ReadLen() As Integer
        Get
            Return mReadLen
        End Get
        Set(ByVal value As Integer)
            mReadLen = value
        End Set
    End Property
    ';触发ondatareceived事件的阈值，可以设置启动事件的阈值
    Public Property RThreshold() As Integer
        Get
            Return mRThreshold
        End Get
        Set(ByVal value As Integer)
            mRThreshold = value
        End Set
    End Property
    'CD线路状态，TRUE、FALSE 高电位置、位电位
    Public ReadOnly Property DSRHolding() As Boolean
        Get
            Return mDSRHolding
        End Get
    End Property
    'CTS线路状态 true,false 高电位，低电位
    Public ReadOnly Property CTSHolding() As Boolean
        Get
            Return mCTSHolding
        End Get
    End Property
    'RI线路状态 true,false 高电位，低电位
    Public ReadOnly Property RIHolding() As Boolean
        Get
            Return mRIHolding
        End Get
    End Property
    'DTR线路状态控制 true,false 高电位，低电位
    Public Property DTR() As Boolean
        Get
            Return mDTR
        End Get
        Set(ByVal value As Boolean)
            If SetDTR(value) Then
                mDTR = value
            End If
        End Set
    End Property
    'RTS线路状态控制 true,false 高电位，低电位
    Public Property RTS() As Boolean
        Get
            Return mRTS
        End Get
        Set(ByVal value As Boolean)
            If SetRTS(value) Then
                mRTS = value
            End If
        End Set
    End Property
#End Region
#End Region
    '事件声明
    Public Event OnDataReceived() '收到RThreshold规定的数据
    Public Event OnCommError(ByVal ErrNo As eCommError) '错误发生
    Public Event OnEvent(ByVal EventMo As eEventMasks) '事件

#Region "方法"
    ''' <summary>
    ''' 打开COM口
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function OpenCOM() As Boolean
        Dim udcb As DCB, irc As Integer
        Dim sdcbstate As String
        If miPort > 0 Then
            '打开通信端口
            mhRS = rs232api.CreateFile("COM" & miPort.ToString(), GENERIC_READ Or GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0)
            If mhRS <> -1 Then
                '清除通信错误
                Dim lperrcode As Integer
                irc = rs232api.ClearCommError(mhRS, lperrcode, Nothing)
                '清除缓冲区
                irc = rs232api.PurgeComm(mhRS, ePurgeBuffers.PXClear Or ePurgeBuffers.txClear)
                '取得通信端口设置值，并填入DCB结构
                irc = rs232api.SetupComm(mhRS, miBufferSize, miBufferSize)
                irc = rs232api.GetCommState(mhRS, udcb)
                '以下将通信端口设置值填入
                udcb.BaudRate = meBaudrate
                udcb.ByteSize = meDataBit
                udcb.Parity = meParity
                udcb.StopBits = meStopBit
                'BITL参数的设置指定
                mDCBBit = 0
                Select Case meHwHandshake '硬件握手状况
                    Case eHwHandShake.hhNone
                        mDCBBit = mDCBBit Or eBitDef.dcb_RtsControldisable
                    Case eHwHandShake.hhRtsOn
                        mDCBBit = mDCBBit Or eBitDef.dcb_RtsControlEnable
                    Case eHwHandShake.hhRtsCts
                        mDCBBit = mDCBBit Or eBitDef.dcb_DtrControlHandshake
                        mDCBBit = mDCBBit Or eBitDef.dcb_OutxCtsFlow
                    Case eHwHandShake.hhDtrOn
                        mDCBBit = mDCBBit Or eBitDef.dcb_DtrControlEnable
                    Case eHwHandShake.hhDtrDsr
                        mDCBBit = mDCBBit Or eBitDef.dcb_DtrControlHandshake
                End Select
                '软件握手状况
                Select Case meSwHandshake
                    Case eSwHandShake.shNone
                    Case eSwHandShake.shXonXoff
                        mDCBBit = mDCBBit Or eBitDef.dcb_InX Or eBitDef.dcb_Outx
                End Select
                udcb.Bits1 = mDCBBit
                irc = rs232api.SetCommState(mhRS, udcb) '将DCB设置进去
                If irc = 0 Then
                    Return False '设置错误返回
                End If
                '打开通信端口时，将DTR，RTS均拉至低电位
                SetDTR(False)
                SetRTS(False)
                mDTR = False
                mRTS = False
                Return True
            Else
                '打开错误
                Return False '打开错误。返回
            End If
        Else
            Return False '串口未定义，返回
        End If
    End Function
    ''' <summary>
    ''' 关闭COM口
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CloseCOM() As Boolean
        If mhRS <> INVALID_HANDLE Then
            mfTimer = False
            Do
                If Timer1.Enabled = False Then Exit Do
                Application.DoEvents()
            Loop
            rs232api.CloseHandle(mhRS)
            mhRS = INVALID_HANDLE
        End If
        Return True
    End Function
#End Region

    Public Sub New()
        Timer1 = New System.Windows.Forms.Timer
        'Timer1 = New System.Windows.Forms.Timer()
        Timer1.Interval = TIMER_INTERVAL
        Timer1.Enabled = False
    End Sub
    '===================================================
    '将字节传出去，    
    '===================================================
    Public Overloads Sub Write(ByVal Buffer As Byte())
        Dim iRc, iBytesWritten As Integer, hOvl As GCHandle
        '-----------------------------------------------------------------
        muOvlW = New OVERLAPPED
        If mhRS.ToInt32 <= 0 Then
            Throw New ApplicationException("请先打开端口")
        Else
            Try
                'hOvl = GCHandle.Alloc(muOvlW, GCHandleType.Pinned)
                'muOvlW.hEvent = CreateEvent(Nothing, 1, 0, Nothing)
                'If muOvlW.hEvent.ToInt32 = 0 Then Throw New ApplicationException("Error creating event for overlapped writing")
                iRc = rs232api.WriteFile(mhRS, Buffer, Buffer.Length, 0, muOvlW)
                If iRc = 0 Then
                    If Marshal.GetLastWin32Error <> ERROR_IO_PENDING Then
                        Throw New ApplicationException("指定失败")
                    Else
                        If rs232api.GetOverlappedResult(mhRS, muOvlW, iBytesWritten, 1) = 0 Then
                            Throw New ApplicationException("写入失败")
                        Else
                            If iBytesWritten <> Buffer.Length Then Throw New ApplicationException("写入字节失败" & iBytesWritten.ToString & " of " & Buffer.Length.ToString)
                        End If
                    End If
                End If
            Finally
                '//Closes handle
                rs232api.CloseHandle(muOvlW.hEvent)
                If (hOvl.IsAllocated = True) Then hOvl.Free()
            End Try
        End If
    End Sub
    Public Overloads Sub Write(ByVal Buffer As String)
        'Dim oEncoder As New System.Text.ASCIIEncoding
        Dim oEnc As New System.Text.UTF8Encoding
        '-------------------------------------------------------------
        Dim aByte() As Byte = oEnc.GetBytes(Buffer)
        Me.Write(aByte)
    End Sub
    '接收字节
    Public Overloads Function Read(ByRef Bytes2Read As Integer, ByRef inputbyte() As Byte) As Boolean
        Dim ireadchars, itoread, irc, lperrors As Integer
        Dim mabtrxbuf As Byte() '接收缓冲区
        '设置读取数据的长度
        If mhRS = -1 Then
            Return False
        Else
            '读取字节
            Try
                Dim cs As COMSTAT
                rs232api.ClearCommError(mhRS, lperrors, cs) '取得缓冲区的字节数
                If mReadLen > 0 Then '是否高置了每次READ命令所读取的字节数
                    If mReadLen > cs.cbInQue Then
                        ireadchars = cs.cbInQue
                    Else
                        ireadchars = mReadLen
                    End If
                Else
                    ireadchars = cs.cbInQue
                End If
                ReDim mabtrxbuf(ireadchars - 1)
                irc = rs232api.ReadFile(mhRS, mabtrxbuf, ireadchars, itoread, Nothing)
                If irc = 0 Then
                    '读取错误
                    Return False
                Else
                    Bytes2Read = ireadchars
                    inputbyte = mabtrxbuf
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End If

    End Function
    '将字符串数据读进来
    Public Overloads Function Read(ByRef char2read As Integer, ByRef inputstring As String) As Boolean
        Dim bytes As Byte()
        If Read(char2read, bytes) Then
            inputstring = System.Text.Encoding.UTF8.GetString(bytes)
            Return True
        Else
            Return False
        End If
    End Function
    '设置DTS状态
    Private Function SetDTR(ByVal fstate As Boolean) As Boolean
        If fstate Then '设置高电位
            If Not rs232api.EscapeCommFunction(mhRS, eLines.SetDtr) Then
                Return False
            End If
        Else
            '设置低电位
            If Not rs232api.EscapeCommFunction(mhRS, eLines.ClearDtr) Then
                Return False
            Else
                Return True
            End If
        End If
        Return True
    End Function
    '设置RTS状态
    Private Function SetRTS(ByVal fstate As Boolean) As Boolean
        If fstate Then '设置高电位
            If Not rs232api.EscapeCommFunction(mhRS, eLines.SetRts) Then
                Return False
            End If
        Else
            '设置低电位
            If Not rs232api.EscapeCommFunction(mhRS, eLines.ClearRts) Then
                Return False
            Else
                Return True
            End If
        End If
        Return True
    End Function
    '取得数字输入线路的状态
    Private Function GetModemStatus() As Integer
        Dim lineresult As Integer
        If Not rs232api.GetCommModemStatus(mhRS, lineresult) Then
            MsgBox("取得信号错误！", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "系统信息")
            Return (-1)
        End If
        Return lineresult
    End Function
    '''定时器TICK事件
    ''' 检查接收的状况。线路状况，错误状况
    ''' 依状况触发预定事件
    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        Dim linestatus, lperrors As Integer
        Dim cs As COMSTAT

        '检查通信端口是否打开
        If Not mfPortOpen Then
            Timer1.Enabled = False
            Exit Sub
        End If
        '检查数据是否达到
        RS232API.ClearCommError(mhRS, lperrors, cs) '取得缓冲区内的字节数
        If mRThreshold > 0 Then
            If cs.cbInQue >= mReadLen Then RaiseEvent OnDataReceived()
        End If
        '检查是否产生错误
        meCommError = lperrors
        If lperrors > 0 Then RaiseEvent OnCommError(lperrors)
        '检查线路状态，决定是否触发事件
        linestatus = GetModemStatus() '取得线路状态
        If linestatus And eModemStatusBits.CarrierDetect Then '比对CD状态
            If Not mCDHolding Then
                meCommEvent = eEventMasks.ReceiveLine
                RaiseEvent OnEvent(eEventMasks.ReceiveLine)

            End If
            mCDHolding = True
        Else
            If mCDHolding Then
                meCommEvent = eEventMasks.ReceiveLine
                RaiseEvent OnEvent(meCommEvent)
            End If
            mCDHolding = False
        End If
        If linestatus And eModemStatusBits.ClearToSendOn Then
            '对比CTS状态
            If Not mCTSHolding Then
                meCommEvent = eEventMasks.ClearToSend
                RaiseEvent OnEvent(eEventMasks.ClearToSend)

            End If
            mCTSHolding = True
        Else
            If mCTSHolding Then
                meCommEvent = eEventMasks.ClearToSend
                RaiseEvent OnEvent(eEventMasks.ClearToSend)
            End If
            mCTSHolding = False
        End If
        If linestatus And eModemStatusBits.DataSetReadOn Then
            '对比DRS状态
            If Not mDSRHolding Then
                meCommEvent = eEventMasks.DataSetReady
                RaiseEvent OnEvent(eEventMasks.DataSetReady)

            End If
            mDSRHolding = True
        Else
            If mDSRHolding Then
                meCommEvent = eEventMasks.DataSetReady
            End If
            mDSRHolding = False
        End If
        If linestatus And eModemStatusBits.RingIndicatorOn Then '比对Ri状态
            If Not mRIHolding Then
                meCommEvent = eEventMasks.Ring
                RaiseEvent OnEvent(eEventMasks.Ring)
            End If
            mRIHolding = True
        Else
            If mRIHolding Then
                meCommEvent = eEventMasks.Ring
                RaiseEvent OnEvent(eEventMasks.Ring)
            End If
            mRIHolding = False
        End If
        '判断定时器状态
        If Not mfTimer Then
            Timer1.Stop() '检查标志，终止定时器的操作
        End If
        Application.DoEvents()

    End Sub
End Class
