# Windows Credential Provider

This project looks to open the oportunity to easily create Windows credential providers for Windows 7, 8 and 10 using managed .NET. Credential providers are COM based, with the interfaces needed for Windows credential providers being written in C++. In .NET, it is possible to do COM interopability and platform invocation, making it possible to make a Windows credential provider in .NET.

Nonetheless, even when using COM interopability and platform invocation, the .NET equivalent interfaces created aren't very friendly for a managed language. This project builds on this by creating an object-oriented and functional API.

## License Information

Versions 1.x are issued under the MIT license. There are no restrictions on the use of the software, except that the MIT license must be attributed to any open-source or commercial redistributions of the software. *The MIT license was chosen for this software to protect commercial interests in the use of the software, whilst at the same time, enabling the software to be used freely in open-source projects.*