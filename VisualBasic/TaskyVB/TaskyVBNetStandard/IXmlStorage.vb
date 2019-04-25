Option Explicit On
''' <summary>
''' Simple interface to allow reading and writing of an 
''' Xml file of serialized Task objects
''' </summary>
Public Interface IXmlStorage

    Function ReadXml(filename As String) As List(Of Task)
    Sub WriteXml(tasks As List(Of Task), filename As String)

End Interface
