
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PCTL_Solver_Core.SystemManagement;

public static class APIExport
{
    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void ReadNetwork(IntPtr s)
    {
        string filePath = Marshal.PtrToStringAnsi(s);
        SystemManager.GetInstance().ReadNetworkFromFile(filePath);
    }


}

