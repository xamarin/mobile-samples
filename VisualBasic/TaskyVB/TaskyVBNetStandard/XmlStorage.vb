Imports System.IO
Imports System.Xml.Serialization
Imports TaskyVBNetStandard

Public Class XmlStorage
    Implements IXmlStorage

    Public Function ReadXml(filename As String) As List(Of TodoItem) Implements IXmlStorage.ReadXml

        If File.Exists(filename) Then
            Dim serializer = New XmlSerializer(GetType(List(Of TodoItem)))

            Using stream = New FileStream(filename, FileMode.Open)
                Return CType(serializer.Deserialize(stream), List(Of TodoItem))
            End Using
        End If

        Return New List(Of TodoItem)()
    End Function

    Public Sub WriteXml(tasks As List(Of TodoItem), filename As String) Implements IXmlStorage.WriteXml

        Dim serializer = New XmlSerializer(GetType(List(Of TodoItem)))

        Using writer = New StreamWriter(filename)
            serializer.Serialize(writer, tasks)
        End Using
    End Sub

End Class
