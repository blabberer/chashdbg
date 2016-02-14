# chashdbg
calling windbg / dbgeng functions in c# 

This is a sample snippet that implements OpenDump , SetOutputCallbacks , WaitForEvent , and Execute 
commnds from windbg using plain c# the code is Fxcop clean as much as possible
compile and linked with windows 7 sdk c# compiler 
csc.exe /nologo /nowin32manifest Debug.cs 
lm and !object \ commands are passed to Execute() dbgeng function on a full kernel dump
