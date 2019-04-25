Option Explicit On
''' <summary>
''' Simple interface to allow reading and writing of an 
''' Xml file of serialized Task objects
''' </summary>
Public Interface IXmlStorage

    Function ReadXml(filename As String) As List(Of TodoItem)
    Sub WriteXml(tasks As List(Of TodoItem), filename As String)

End Interface
