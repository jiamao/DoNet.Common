Imports System.Runtime.InteropServices
Public Class RS232API
    Dim rs232 As cRS232
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetCommState(ByVal hCommDev As IntPtr, ByRef lpDCB As RS232.cRS232.DCB) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function GetCommState(ByVal hCommDev As IntPtr, ByRef lpDCB As RS232.cRS232.DCB) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True, CharSet:=CharSet.Auto)> Public Shared Function BuildCommDCB(ByVal lpDef As String, ByRef lpDCB As RS232.cRS232.DCB) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetupComm(ByVal hFile As IntPtr, ByVal dwInQueue As Int32, ByVal dwOutQueue As Int32) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetCommTimeouts(ByVal hFile As IntPtr, ByRef lpCommTimeouts As RS232.cRS232.COMMTIMEOUTS) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function GetCommTimeouts(ByVal hFile As IntPtr, ByRef lpCommTimeouts As RS232.cRS232.COMMTIMEOUTS) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function ClearCommError(ByVal hFile As IntPtr, ByRef lpErrors As Int32, ByRef lpComStat As RS232.cRS232.COMSTAT) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function PurgeComm(ByVal hFile As IntPtr, ByVal dwFlags As Int32) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function EscapeCommFunction(ByVal hFile As IntPtr, ByVal ifunc As Int32) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function WaitCommEvent(ByVal hFile As IntPtr, ByRef Mask As RS232.cRS232.eEventMasks, ByRef lpOverlap As RS232.cRS232.OVERLAPPED) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function WriteFile(ByVal hFile As IntPtr, ByVal Buffer As Byte(), ByVal nNumberOfBytesToWrite As Integer, ByRef lpNumberOfBytesWritten As Integer, ByRef lpOverlapped As RS232.cRS232.OVERLAPPED) As Integer
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function ReadFile(ByVal hFile As IntPtr, <Out()> ByVal Buffer As Byte(), ByVal nNumberOfBytesToRead As Integer, ByRef lpNumberOfBytesRead As Integer, ByRef lpOverlapped As RS232.cRS232.OVERLAPPED) As Integer
    End Function
    <DllImport("kernel32.dll", SetlastError:=True, CharSet:=CharSet.Auto)> Public Shared Function CreateFile(ByVal lpFileName As String, ByVal dwDesiredAccess As Integer, ByVal dwShareMode As Integer, ByVal lpSecurityAttributes As Integer, ByVal dwCreationDisposition As Integer, ByVal dwFlagsAndAttributes As Integer, ByVal hTemplateFile As Integer) As IntPtr
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function CloseHandle(ByVal hObject As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function GetCommModemStatus(ByVal hFile As IntPtr, ByRef lpModemStatus As Int32) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetEvent(ByVal hEvent As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True, CharSet:=CharSet.Auto)> Public Shared Function CreateEvent(ByVal lpEventAttributes As IntPtr, ByVal bManualReset As Int32, ByVal bInitialState As Int32, ByVal lpName As String) As IntPtr
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function WaitForSingleObject(ByVal hHandle As IntPtr, ByVal dwMilliseconds As Int32) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function GetOverlappedResult(ByVal hFile As IntPtr, ByRef lpOverlapped As RS232.cRS232.OVERLAPPED, ByRef lpNumberOfBytesTransferred As Int32, ByVal bWait As Int32) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetCommMask(ByVal hFile As IntPtr, ByVal lpEvtMask As Int32) As Int32
    End Function
    <DllImport("kernel32.dll", SetlastError:=True, CharSet:=CharSet.Auto)> Public Shared Function GetDefaultCommConfig(ByVal lpszName As String, ByRef lpCC As RS232.cRS232.COMMCONFIG, ByRef lpdwSize As Integer) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function SetCommBreak(ByVal hFile As IntPtr) As Boolean
    End Function
    <DllImport("kernel32.dll", SetlastError:=True)> Public Shared Function ClearCommBreak(ByVal hFile As IntPtr) As Boolean
    End Function
End Class
