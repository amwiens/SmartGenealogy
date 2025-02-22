using System.Text;

using ExifLibrary;

namespace SmartGenealogy.Core.Helper;

public class MyTiffFile(MemoryStream stream, Encoding encoding) : TIFFFile(stream, encoding);