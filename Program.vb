Imports System
Imports System.Xml

Module Program
    Dim xmlPath As String = "./data.xml"

    Sub Main(args As String())
        Dim doc As XmlDocument = LoadXML(xmlPath)
        Dim categories As List(Of Category) = Xml2category(doc)

        For Each c1 As Category In categories 
            Console.WriteLine("1st category: " + c1.Name) 
            Console.WriteLine("1st category children: {") 
            If c1.Children.Count = 0 Then
                Continue For
            End If

            For Each c2 As Category In c1.Children 
                Console.WriteLine(ControlChars.Tab + "2nd category: " + c2.Name) 

                If c2.Children.Count = 0 Then
                    Continue For
                End If

                Console.WriteLine(ControlChars.Tab + "2nd category children: {") 
                For Each c3 As Category In c2.Children 
                    Console.WriteLine(ControlChars.Tab + ControlChars.Tab + "3rd category: " + c3.Name) 
                Next
                Console.WriteLine(ControlChars.Tab + "}") 
            Next
            Console.WriteLine("}") 
        Next
    End Sub
End Module
