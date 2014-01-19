Frost Resource Package file format (.frp files)
===============================================

Frost Resource Package files can contain any type of resource.
Each resource has, at minimum, a name and ID.
There are no restrictions on what a resource can be named.
However, it is recommended to use a delimiter, like a forward slash /, to logically group resources.
The resource ID should be unique across all known resources, including resources with the same name.

Resources may have additional optional parameters.
All parameters for resources are defined in the header of the resource file.
Resource package files are stored as big-endian.

Header
------

There are two parts of the resource package file header: the file info and the resource info.

### File Info ###

The file info section of the header consists of 8 bytes.
Those bytes represent the following values:
* byte 1 - File version number (1 to 255)
* bytes 2-3 - File options (see below)
* byte 4 - File block size (0 to 255)
* bytes 5-8 Size (in bytes) of the following resource info

#### Files Options ####

Resource package files can have the following options:

* 0x0000 - None
* 0x0001 - Encrypt file header (the resource info following the initial 8 bytes will be encrypted)

### Resource Info ###

The resource info immediately follows the file info section of the header
It is stored as a TNT (Typed-node tree) container (an explanation of TNT data is not given in this document).
The resource info is compressed using Zlib deflate.
The size of the compressed data is given by bytes 5-8 in the header.
The structure of the TNT container should be:
- Container
-- (Complex) Root node
--- (String) name:        Name of the resource pack
--- (String) description: Brief description of what the resource pack contains
--- (String) creator:     Name (and possibly email or web address) of the package creator
--- (List of Complex) entries: Resources in the package file
---- (GUID)   id:     Globally unique identifier of the resource (should be unique across all known resources, even if they have the same name)
---- (String) name:   Name of the resource (must be unique in the current package file)
---- (Int)    offset: Index of the block where the resource data starts offset from the header block (first resource after the header has an offset of 0)
---- (Int)    size:   Size in bytes of the packed resource (compressed and encrypted)
These are just the minimum expected nodes, additional nodes can be added with no effect.

Resources
---------

All resources stored in package files are compressed using Zlib deflate.
The location of where a resource starts and its packed size are given in the resource info part of the file header.
