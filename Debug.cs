using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
[assembly: ComVisible(false)] // to satisfy fxcop
[assembly: CLSCompliant(true)] // to satisfy fxcop
[assembly:AssemblyVersionAttribute("1.0.0")] // to satisfy fxcop
// implementing only Opendump and SetOutputCallbacks in IDebugClient
// implementing only Execute and WaitForEvent in IDebugControl
// implementing only Output in IDebugOutputCallBacks
// others are dummy (not even implementing Unimplemented exception)
// the dummies are required to ensure the methods are on the correct index 
// once we get the interfaces we open a dump file named memory.dmp 
// dump file created by issuing .crash .reboot on a vm running xp sp3 with 128 mb
// allocated ram (dump file is 127 mb)
// execute lm (list modules windbg bang command)
//execute !object \ 
// compiled and linked with win7 sdk c# compiler 
// csc.exe /nologo /nowin32manifest
// executed on a win 7 32 bit machine;

namespace Test
{  
  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
  Guid("27fe5639-8407-4f47-8364-ee118fb08ac8")]
  public interface IDebugClient
  {
    int D00(); int D01(); int D02(); int D03(); int D04(); 
	int D05(); int D06(); int D07(); int D08(); int D09(); 
	int D10(); int D11(); int D12(); int D13(); int D14(); 
	int D15();
	[PreserveSig]
    int OpenDumpFile(
		[In, MarshalAs(UnmanagedType.LPStr)] string DumpFile);
	int D17(); int D18(); int D19(); int D20(); int D21(); 
	int D22(); int D23(); int D24(); int D25(); int D26(); 
	int D27(); int D28(); int D29(); int D30();
    [PreserveSig]
    int SetOutputCallbacks(
        [In] IDebugOutputCallbacks callbacks);
  }
  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
  Guid("5182e668-105e-416e-ad92-24ef800424ba")]
  public interface IDebugControl
  {
    int D01(); int D02(); int D03(); int D04(); int D05(); 
    int D06(); int D07(); int D08(); int D09(); int D10();
    int D11(); int D12(); int D13(); int D14(); int D15(); 
    int D16(); int D17(); int D18(); int D19(); int D20();
    int D21(); int D22(); int D23(); int D24(); int D25(); 
    int D26(); int D27(); int D28(); int D29(); int D30();
    int D31(); int D32(); int D33(); int D34(); int D35(); 
    int D36(); int D37(); int D38(); int D39(); int D40();
    int D41(); int D42(); int D43(); int D44(); int D45(); 
    int D46(); int D47(); int D48(); int D49(); int D50();
    int D51(); int D52(); int D53(); int D54(); int D55(); 
    int D56(); int D57(); int D58(); int D59(); int D60();
    int D61(); int D62(); int D63();
    [PreserveSig]
    int Execute(
        [In] int outputControl,
        [In, MarshalAs(UnmanagedType.LPStr)] string command,
        [In] int flake);
    int D65(); int D66(); int D67(); int D68(); int D69(); int D70();
    int D71(); int D72(); int D73(); int D74(); int D75(); 
    int D76(); int D77(); int D78(); int D79(); int D80();
    int D81(); int D82(); int D83(); int D84(); int D85(); 
    int D86(); int D87(); int D88(); int D89(); int D90();
    [PreserveSig]
    int WaitForEvent(
        [In] int wait,
        [In] int timeout);
  }
  [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
  Guid("4bf58045-d654-4c40-b0af-683090f356dc")]
  public interface IDebugOutputCallbacks
  {
    [PreserveSig]
    int Output(
        [In] int mask,
        [In, MarshalAs(UnmanagedType.LPStr)] string text);
  }
}
internal  static class NativeMethods
{
  internal static Test.IDebugClient g_Client = null;
  internal static Test.IDebugControl g_Control = null;
  internal static Test.IDebugOutputCallbacks g_Output = null;
  [DllImport("kernel32.dll" ,CharSet = CharSet.Ansi,
  BestFitMapping=false,ThrowOnUnmappableChar=true)]
  internal static extern IntPtr LoadLibrary( string dllpath);
  [DllImport("kernel32.dll", CharSet = CharSet.Ansi,
  BestFitMapping=false,ThrowOnUnmappableChar=true)]
  internal static extern IntPtr GetProcAddress(IntPtr dllbase, 
    string functionname);
  internal delegate uint DebugCreate(
          ref Guid interfaceId,
          [MarshalAs(UnmanagedType.IUnknown)] out object face);
}
class Text : Test.IDebugOutputCallbacks
{
  public int Output(int Mask, string Text)
  {
    Console.Write(Text);
    return 0;
  }
}
namespace ConsoleApplication1
{
  class Program
  {
    static void Main()
    {
      Guid iid = new Guid("27fe5639-8407-4f47-8364-ee118fb08ac8");
      Object iface = null;
      System.IntPtr moduleHandle = NativeMethods.LoadLibrary("Dbgeng.dll");
      System.IntPtr hProc = NativeMethods.GetProcAddress(
      moduleHandle, "DebugCreate");
      NativeMethods.DebugCreate debugCreate = (NativeMethods.DebugCreate)Marshal.
        GetDelegateForFunctionPointer(hProc, typeof(NativeMethods.DebugCreate));
      debugCreate(ref iid, out iface);
      NativeMethods.g_Client = (Test.IDebugClient)iface;
      NativeMethods.g_Control = (Test.IDebugControl)iface;
      NativeMethods.g_Output = new Text();
	  int a = NativeMethods.g_Client.SetOutputCallbacks(NativeMethods.g_Output);
	  a = NativeMethods.g_Client.OpenDumpFile("memory.dmp");
	  a = NativeMethods.g_Control.WaitForEvent(0, 
      System.Threading.Timeout.Infinite);
	  a = NativeMethods.g_Control.Execute(0, "lm", 0);
	  a = NativeMethods.g_Control.Execute(0, "!object \\", 0);
	}
  }
}