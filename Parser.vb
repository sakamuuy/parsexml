Imports System.Xml

Module Parser
  Function LoadXML(path As String) As XmlDocument
    Dim doc As XmlDocument = New XmlDocument()
    doc.Load(path)
    Console.WriteLine("Loaded: " + doc.DocumentElement.FirstChild.OuterXml)

    return doc
  End Function

  Function Xml2Category(doc As XmlDocument) As List(Of Category) 
    Dim root As XmlNode = doc.DocumentElement
    Dim facets As XmlNode = root.SelectNodes("facets")(0)
    Dim categoryHt As Hashtable  = New Hashtable
    Dim ret = New List(Of Category)()

    If isNothing(facets) Then
      Console.Error.WriteLine("facets is not declared.")
    End If

    If facets.HasChildNodes <> True Then
      Console.Error.WriteLine("facets has no children.")
    End If
    
    For Each node As XmlElement In facets
      Dim nameList As String() = node.GetAttribute("name").split("|")
      Dim category1 As Category = Nothing
      Dim category2 As Category = Nothing
      Dim category3 As Category = Nothing

      For i As Integer = 0 To nameList.Length - 1
        Dim name As String = nameList(i)

        If i = 0 Then 
          category1 = atMostOne(categoryHt, name, node)

        ElseIf i = 1 And category1 IsNot Nothing = True Then
          category2 = atMostOneFromParent(category1, name, node)

        ElseIf i = 2 And category1 IsNot Nothing = True And category2 IsNot Nothing = True Then
          category3 = atMostOneFromParent(category2, name, node)

        End If
      Next
    Next 

    For Each c As Category In categoryHt.Values
      ret.Add(c)
    Next

    return ret
  End Function


  Private Function atMostOne(categoryHt As Hashtable, name As String, node As XmlElement) As Category
    Dim c As Category = CType(categoryHt(name), Category) 

    If isNothing(c) Then
      c = new Category() 
      c.Name = name
      c.Value = node.GetAttribute("value")
      c.Count = node.GetAttribute("count")
      c.ChildrenHt = New Hashtable
      c.Children = New List(Of Category)()

      categoryHt(name) = c
    End If

    return c
  End Function


  Private Function atMostOneFromParent(parent As Category, name As String, node As XmlElement) As Category
    Dim c As Category = CType(parent.ChildrenHt(name), Category)

    If isNothing(c) Then
      c = new Category() 
      c.Name = name
      c.Value = node.GetAttribute("value")
      c.Count = node.GetAttribute("count")
      c.ChildrenHt = New Hashtable
      c.Children = New List(Of Category)()

      parent.ChildrenHt(name) = c
      parent.Children.Add(c)
    End If

    return c
  End Function
End Module
