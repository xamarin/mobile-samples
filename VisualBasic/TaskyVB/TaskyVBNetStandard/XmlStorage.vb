Imports System.IO
Imports System.Xml.Serialization
Imports TaskyVBNetStandard

Public Class XmlStorage
    Implements IXmlStorage

    Public Function ReadXml(filename As String) As List(Of Task) Implements IXmlStorage.ReadXml

        If File.Exists(filename) Then
            Dim serializer = New XmlSerializer(GetType(List(Of Task)))

            Using stream = New FileStream(filename, FileMode.Open)
                Return CType(serializer.Deserialize(stream), List(Of Task))
            End Using
        End If

        Return New List(Of Task)()
    End Function

    Public Sub WriteXml(tasks As List(Of Task), filename As String) Implements IXmlStorage.WriteXml

        Dim serializer = New XmlSerializer(GetType(List(Of Task)))

        Using writer = New StreamWriter(filename)
            serializer.Serialize(writer, tasks)
        End Using
    End Sub

End Class
