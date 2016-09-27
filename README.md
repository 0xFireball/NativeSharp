
# NativeSharp

Open source C# to C++ transpiler (NativeSharp, based on Cs2Cpp)

## Information

NativeSharp is heavily based on [Cs2Cpp](https://github.com/ASDAlexander77/cs2cpp).



# License

Cs2Cpp is licensed under the MIT license.

# Quick Start

Prerequisite: CMake, .NET 4.6, GCC 5.0+ or Microsoft Visual C++ 2015 

1. Build Project

```
cd Il2Native
MSBuild Il2Native.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

2. Build CoreLib (aka mscorlib)

```
cd CoreLib
MSBuild CoreLib.csproj /p:Configuration=Release /p:Platform="AnyCPU"
```

3. Create a temporary folder to build projects/files

```
cd ..\..
mkdir playground
cd playground
```

4. Generating CoreLib C++ project

```
..\Il2Native\Il2Native\bin\Debug\Cs2Cpp.exe ..\Il2Native\CoreLib\CoreLib.csproj
```

5. Compile it

```
cd CoreLib
build_prerequisite_vs2015_debug.bat 
build_vs2015_debug.bat
```

Now you have compiled CoreLib (mscorelib)

6) Compile HelloWorld.cs

```
cd ..
```

create file HelloWorld.cs

```C#
using System;

class X {
	public static int Main (string [] args)
	{
		Console.WriteLine ("Hello, World!");
		return 0;
	}
}
```

```
..\Il2Native\Il2Native\bin\Debug\Cs2Cpp.exe HelloWorld.cs /corelib:..\Il2Native\CoreLib\bin\Release\CoreLib.dll
cd HelloWorld
build_vs2015_release.bat
```

Now you have HelloWorld.exe